from pathlib import Path
import math
import re
import html
import json

from reportlab.pdfgen import canvas
from reportlab.pdfbase import pdfmetrics
from reportlab.pdfbase.ttfonts import TTFont
from reportlab.lib import colors
from reportlab.lib.enums import TA_CENTER, TA_LEFT
from reportlab.lib.pagesizes import A4
from reportlab.lib.styles import ParagraphStyle
from reportlab.lib.units import mm
from reportlab.platypus import (
    SimpleDocTemplate, Paragraph, Spacer, Table, TableStyle,
    Image, PageBreak, KeepTogether,
)
from PIL import Image as PILImage

ROOT = Path(r'E:\study\informationdefense')
LAB = ROOT / 'lab1'
OUT = ROOT / 'output' / 'pdf'
OUT.mkdir(parents=True, exist_ok=True)
PDF = OUT / 'Лабораторная_работа_1_Восстановление_паролей.pdf'
MD = LAB / 'Отчёт_лабораторная_1.md'
V = 21_021_990
ACTUAL = 476

font_dir = Path(r'C:\Windows\Fonts')
for name, filename in [('TNR', 'times.ttf'), ('TNR-Bold', 'timesbd.ttf'),
                       ('TNR-Italic', 'timesi.ttf'), ('TNR-BoldItalic', 'timesbi.ttf')]:
    pdfmetrics.registerFont(TTFont(name, str(font_dir / filename)))
pdfmetrics.registerFontFamily('TNR', normal='TNR', bold='TNR-Bold',
                             italic='TNR-Italic', boldItalic='TNR-BoldItalic')

styles = {
    'title': ParagraphStyle('Title', fontName='TNR-Bold', fontSize=19,
                            leading=23, textColor=colors.black, spaceAfter=4),
    'subtitle': ParagraphStyle('Subtitle', fontName='TNR', fontSize=15,
                               leading=19, spaceAfter=13, textColor=colors.black),
    'h': ParagraphStyle('Heading', fontName='TNR-Bold', fontSize=13,
                        leading=16, spaceBefore=12, spaceAfter=7,
                        keepWithNext=True, textColor=colors.black),
    'h2': ParagraphStyle('Question', fontName='TNR-Bold', fontSize=11.5,
                         leading=14, spaceBefore=10, spaceAfter=4,
                         keepWithNext=True, textColor=colors.black),
    'body': ParagraphStyle('Body', fontName='TNR', fontSize=11.5,
                           leading=15, spaceAfter=7, textColor=colors.black),
    'small': ParagraphStyle('Small', fontName='TNR', fontSize=10,
                            leading=12.5, spaceAfter=6, textColor=colors.black),
    'eq': ParagraphStyle('Equation', fontName='TNR', fontSize=12,
                         leading=17, alignment=TA_CENTER, spaceBefore=3, spaceAfter=7),
    'cell': ParagraphStyle('Cell', fontName='TNR', fontSize=10,
                           leading=12, spaceAfter=0),
    'cellsmall': ParagraphStyle('CellSmall', fontName='TNR', fontSize=9.5,
                                leading=11.5, spaceAfter=0),
    'headcell': ParagraphStyle('HeadCell', fontName='TNR-Bold', fontSize=9.5,
                               leading=11.5, spaceAfter=0),
    'caption': ParagraphStyle('Caption', fontName='TNR', fontSize=10,
                              leading=12, alignment=TA_CENTER, spaceBefore=4,
                              spaceAfter=13),
}

story = []
md = []

def plain(s):
    s = re.sub(r'<super>(.*?)</super>', r'^\1', s)
    s = re.sub(r'<sub>(.*?)</sub>', r'_\1', s)
    s = re.sub(r'<br\s*/?>', ' ', s)
    return html.unescape(re.sub('<[^>]+>', '', s))

def p(s, style='body', md_text=None):
    story.append(Paragraph(s, styles[style]))
    md.append(md_text if md_text is not None else plain(s))
    md.append('')

def heading(s, level=1):
    story.append(Paragraph(s, styles['h' if level == 1 else 'h2']))
    md.extend([('#' * (level + 1)) + ' ' + s, ''])

def table(headers, rows, widths, small=False):
    cellstyle = styles['cellsmall' if small else 'cell']
    cells = [[Paragraph(c, styles['headcell']) for c in headers]]
    cells.extend([[Paragraph(str(c), cellstyle) for c in row] for row in rows])
    t = Table(cells, colWidths=widths, repeatRows=1, hAlign='LEFT')
    t.setStyle(TableStyle([
        ('GRID', (0, 0), (-1, -1), 0.5, colors.HexColor('#D9D9D9')),
        ('BACKGROUND', (0, 0), (-1, 0), colors.HexColor('#E8EDF3')),
        ('VALIGN', (0, 0), (-1, -1), 'MIDDLE'),
        ('LEFTPADDING', (0, 0), (-1, -1), 7),
        ('RIGHTPADDING', (0, 0), (-1, -1), 7),
        ('TOPPADDING', (0, 0), (-1, -1), 6),
        ('BOTTOMPADDING', (0, 0), (-1, -1), 6),
    ]))
    story.extend([t, Spacer(1, 8)])
    md.append('| ' + ' | '.join(plain(c) for c in headers) + ' |')
    md.append('|' + '|'.join('---' for _ in headers) + '|')
    for row in rows:
        md.append('| ' + ' | '.join(plain(str(c)) for c in row) + ' |')
    md.append('')

def page():
    story.append(PageBreak())

def integer(n):
    return f'{n:,}'.replace(',', '\u00a0')

def duration(seconds):
    n = int(round(seconds))
    d, n = divmod(n, 86400)
    h, n = divmod(n, 3600)
    m, s = divmod(n, 60)
    out = []
    if d: out.append(f'{d} сут')
    if h: out.append(f'{h} ч')
    if m: out.append(f'{m} мин')
    if s or not out: out.append(f'{s} с')
    return ' '.join(out)

def figure(filename, caption):
    path = LAB / filename
    with PILImage.open(path) as im:
        width, height = im.size
    w = 147 * mm
    image = Image(str(path), width=w, height=w * height / width)
    image.hAlign = 'CENTER'
    block = [image, Paragraph(caption, styles['caption'])]
    story.append(KeepTogether(block))
    md.extend([f'![{caption}](<{path.as_posix()}>)', ''])


# Page 1: what was actually done.
p('Лабораторная работа 1', 'title')
p('Восстановление паролей в Windows XP', 'subtitle')
md[0] = '# Лабораторная работа 1'
p('В работе я проверил, как длина цифрового пароля влияет на время его восстановления. '
  'Основной результат: пароль <b>4827193659</b> удалось восстановить за <b>7 минут 56 секунд</b>. '
  'Это укладывается в интервал 5-15 минут, предложенный в задании.')

heading('Что использовал')
p('Oracle VirtualBox 7.1.6, Windows XP 32 бит и Cain &amp; Abel. '
  'Для опытов создал отдельные учебные учётные записи lab_test, lab_speed и lab_final. '
  'Во всех запусках выбрал полный перебор NTLM-хеша и цифровой алфавит 0123456789.')
p('Цель - измерить скорость перебора, подобрать параметры пароля для приемлемого времени '
  'восстановления и рассчитать примеры для часа, суток, недели, месяца и года.')

heading('Как проходили опыты')
p('<b>Сначала проверил короткий пароль.</b> Для lab_test задал 573921 и длину ровно 6 символов. '
  'Получилось 1 000 000 вариантов. Программа нашла пароль и показала «1 of 1 hashes cracked». '
  'Время этого короткого запуска отдельно не измерял. Настройки и результат приведены на рисунках 1 и 2.')
p('<b>Затем измерил скорость.</b> Для lab_speed использовал учебный пароль 937184620583 и '
  'длину 12 символов. Такой запуск нужен был для замера скорости; до полного завершения его '
  'не доводил. Во время работы поле Key Rate показывало <b>21 021 990 Pass/Sec</b> '
  '(рисунки 3 и 4).')
p('<b>В основном опыте</b> для lab_final задал пароль 4827193659 и длину 10. '
  'При найденной скорости полный проход оценивался примерно в 8 минут. '
  'После запуска засёк время секундомером. Пароль был найден, итоговое время - '
  '<b>476 секунд</b> (рисунки 5 и 6).')

table(['Опыт', 'Длина', 'Число вариантов', 'Результат'], [
    ['Короткий пароль', '6', '10<super>6</super>', '573921 найден'],
    ['Замер скорости', '12', '10<super>12</super>', '21 021 990 проверок/с'],
    ['Основной', '10', '10<super>10</super>', '4827193659 найден<br/>за 7 мин 56 с'],
], [115, 43, 111, 224])
p('Фактическое время основного опыта записано по секундомеру. Итоговый скриншот '
  'подтверждает найденный пароль; само поле Key Rate после завершения уже пустое.', 'small')

# Page 2: calculation model, all five time horizons, explicit examples.
page()
heading('Расчёты времени перебора')
p('Для расчётов взял измеренную скорость V = 21 021 990 проверок/с. '
  'Она считается постоянной; это упрощение для оценки полного прохода пространства поиска.')
p('<i>N</i> = <i>A</i><super><i>L</i></super>; '
  '<i>T</i><sub>max</sub> = <i>N</i> / <i>V</i>; '
  '<i>T</i><sub>ср</sub> ≈ <i>N</i> / (2<i>V</i>); '
  '<i>H</i> = log<sub>2</sub><i>N</i>', 'eq')
p('Здесь A - размер алфавита, L - фиксированная длина, N - число кандидатов, '
  'H - энтропия равномерного выбора в битах. Среднее время относится к равновероятному '
  'паролю и перебору без повторений. Для маски N равно произведению размеров алфавитов '
  'отдельных позиций.', 'small')
p('Для основного опыта: N = 10<super>10</super>; '
  'T<sub>max</sub> = 10<super>10</super> / 21 021 990 ≈ <b>475,69 с</b>; '
  'H = 10 log<sub>2</sub>10 ≈ <b>33,22 бита</b>. '
  'Измеренные 476 с близки к оценке полного прохода. При этом время нахождения конкретного '
  'пароля зависит от его позиции в переборе, а скорость между запусками может меняться.', 'small')

p('Чтобы полный проход занимал заданное время t, нужно пространство N = Vt. '
  'Месяц здесь принят равным 30 суткам, год - 365 суткам.', 'small')
horizons = [('1 час', 3600), ('1 сутки', 86400), ('1 неделя', 604800),
            ('1 месяц', 2592000), ('1 год', 31536000)]
table(['Срок', 'Время t в секундах', 'Число вариантов Vt', 'H в битах'], [
    [label, integer(t), integer(V*t), f'{math.log2(V*t):.2f}'.replace('.', ',')]
    for label, t in horizons
], [67, 104, 235, 87], small=True)

p('Ниже подобрал примеры масок с близкими сроками. Обозначения: d - цифры (10), '
  'l - строчные латинские буквы (26), a - строчные буквы и цифры (36), '
  'b - латинские буквы обоих регистров (52), c - буквы обоих регистров и цифры (62), '
  'p - печатные ASCII вместе с пробелом (95). Степень показывает число позиций; '
  'знак + означает соединение частей.', 'small')

examples = [
    ('Около часа', 'a<super>7</super>', 'k7v2m9q', 36**7),
    ('Около суток', 'a<super>2</super> + c<super>4</super> + p', 'k7G2mQ!', 36**2 * 62**4 * 95),
    ('Около недели', 'c<super>4</super> + p<super>3</super>', 'G7mQ!#?', 62**4 * 95**3),
    ('Около месяца', 'l<super>9</super> + d', 'qvmrtnkza7', 26**9 * 10),
    ('Около года', 'b + c<super>4</super> + p<super>3</super>', 'qG7mQ!#?', 52 * 62**4 * 95**3),
]
table(['Ориентир', 'Маска и пример строки', 'Число вариантов N', 'Полный проход N/V'], [
    [target, mask+'<br/>'+sample, integer(n), duration(n/V)]
    for target, mask, sample, n in examples
], [72, 160, 140, 121], small=True)
p('Эти пять сроков рассчитаны, а не измерены отдельными запусками. '
  'Маска предполагается известной заранее, а символы - случайными и равновероятными '
  'в своих алфавитах. Пример строки сам по себе не гарантирует такой срок нахождения. '
  'Для оценки среднего времени значения полного прохода нужно примерно разделить на два.', 'small')

# Page 3: control questions and conclusions in a simple student voice.
page()
heading('Ответы на контрольные вопросы')
heading('В каком виде хранятся пароли и зачем нужен SYSKEY', 2)
p('Для локальных учётных записей Windows XP использует SAM. Файл находится в '
  '%SystemRoot%\\System32\\Config\\SAM. В нём хранятся защищённые представления хешей, '
  'а не открытый текст паролей. NT-хеш - MD4 от пароля в кодировке UTF-16LE, длина результата 128 бит. '
  'Он не содержит индивидуальной соли.')
p('LM - старая схема: пароль переводится в верхний регистр, дополняется до 14 байт и делится '
  'на две части по 7 байт. Каждая часть задаёт DES-ключ для обработки фиксированной строки; '
  'два результата объединяются в 16 байт. Потеря регистра и независимые половины упрощают поиск. '
  'SYSKEY в старых Windows дополнительно защищал хеши при хранении с помощью системного ключа. '
  'Это отдельная защита хранения, а не соль NT-хеша.')

heading('Основные режимы восстановления паролей', 2)
table(['Режим', 'Плюс', 'Минус'], [
    ['По словарю', 'Быстро проверяет вероятные слова', 'Пароля может не быть в словаре'],
    ['Полный перебор', 'Покрывает всё заданное пространство', 'Время быстро растёт с длиной'],
    ['Гибридный', 'Проверяет слова с добавлениями и заменами', 'Зависит от словаря и правил'],
    ['По маске', 'Сокращает поиск по известной структуре', 'Неверная маска исключит нужный пароль'],
], [95, 196, 202], small=True)
p('В методичке также упомянуты радужные таблицы: часть вычислений сделана заранее, '
  'но таблицы требуют места и подходят только к определённым алгоритмам и пространствам. '
  'Индивидуальная соль мешает применять одну таблицу сразу ко всем паролям.', 'small')

heading('Какой пароль можно считать простым или сложным', 2)
p('В этой работе простой пароль - тот, который удаётся восстановить за приемлемое время '
  'на выбранном компьютере. Сложный потребует заметно больше времени. В методичке приведены '
  'исторические рекомендации Microsoft: длина от 7 символов, разные группы символов и отсутствие '
  'личных данных и популярных слов. Для реальной стойкости важны длина и случайность: '
  'предсказуемое слово с цифрой остаётся удобной целью для словарных правил.')

heading('Что такое подсоленный пароль', 2)
p('Обычно так называют хранение результата функции от пароля с добавлением индивидуальной '
  'соли. Соль сохраняют вместе с результатом; секретной она быть не обязана. '
  'Разные соли дают разные результаты для одинаковых паролей и мешают использовать готовые '
  'таблицы для массовой проверки. Для современного хранения нужна также затратная функция, '
  'например Argon2id, scrypt или PBKDF2. У обычного NT-хеша соли нет.')

heading('Вывод')
p('Я проверил восстановление короткого цифрового пароля, измерил скорость и провёл основной '
  'опыт с длиной 10 символов. Пароль 4827193659 был найден за 7 минут 56 секунд. '
  'При увеличении длины цифрового пароля на один символ число вариантов возрастает в 10 раз: '
  'для длины 12 полный проход уже оценивается примерно в 13,21 часа. '
  'Расчёты также показали, что знание структуры пароля заметно меняет объём поиска. '
  'Мои результаты относятся к NT-хешам и выбранному режиму; для другого алгоритма скорость '
  'нужно измерять заново.')
p('Источник задания: методические указания «Восстановление паролей в Windows XP», '
  'лабораторная работа 1, с. 5-7. Скриншоты опытов приведены далее.', 'small')

# Original screenshots, unchanged, in reading order.
page()
heading('Скриншоты предварительного опыта')
figure('Снимок экрана 2026-10-19 222641.png',
       'Рисунок 1 - NTLM, цифровой алфавит и длина 6')
figure('Снимок экрана 2026-10-19 222724.png',
       'Рисунок 2 - Восстановлен пароль 573921')

page()
heading('Скриншоты замера скорости')
figure('Снимок экрана 2026-10-19 223441.png',
       'Рисунок 3 - Для замера выбрана длина 12')
figure('Снимок экрана 2026-10-19 223659.png',
       'Рисунок 4 - Key Rate показывает 21 021 990 Pass/Sec')

page()
heading('Скриншоты основного опыта')
figure('Снимок экрана 2026-10-19 225455.png',
       'Рисунок 5 - Цифровой алфавит и длина 10')
figure('Снимок экрана 2026-10-19 230329.png',
       'Рисунок 6 - Восстановлен пароль 4827193659')

def footer(c, doc):
    c.saveState()
    c.setFont('TNR', 9)
    c.setFillColor(colors.HexColor('#555555'))
    c.drawCentredString(A4[0]/2, 9*mm, str(doc.page))
    c.restoreState()

doc = SimpleDocTemplate(
    str(PDF), pagesize=A4, rightMargin=18*mm, leftMargin=18*mm,
    topMargin=15*mm, bottomMargin=15*mm,
    title='Восстановление паролей в Windows XP', author='',
    subject='Лабораторная работа 1',
)
doc.build(story, onFirstPage=footer, onLaterPages=footer)
MD.write_text('\n'.join(md), encoding='utf-8')

# Verify the arithmetic and that each example matches its stated mask.
alphabets = {
    'd': '0123456789',
    'l': 'abcdefghijklmnopqrstuvwxyz',
    'a': 'abcdefghijklmnopqrstuvwxyz0123456789',
    'b': 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ',
    'c': 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789',
    'p': ''.join(chr(c) for c in range(32, 127)),
}
masks = ['a'*7, 'a'*2+'c'*4+'p', 'c'*4+'p'*3, 'l'*9+'d', 'b'+'c'*4+'p'*3]
for (_, _, sample, n), mask in zip(examples, masks):
    assert len(sample) == len(mask)
    assert all(ch in alphabets[a] for ch, a in zip(sample, mask))
    assert math.prod(len(alphabets[a]) for a in mask) == n

from pypdf import PdfReader
reader = PdfReader(str(PDF))
text = '\n'.join(page.extract_text() for page in reader.pages)
for token in ['4827193659', '573921', '476', '21 021 990', '7 минут 56 секунд']:
    assert token in text, token
assert text.count('Рисунок ') == 6
assert '[ФИО' not in text and 'Вставить' not in text
print(json.dumps({
    'pdf': str(PDF), 'markdown': str(MD), 'pages': len(reader.pages),
    'page_text_chars': [len(page.extract_text()) for page in reader.pages],
    'examples': [{'sample': sample, 'N': n, 'seconds': n/V}
                 for _, _, sample, n in examples],
}, ensure_ascii=False, indent=2))
