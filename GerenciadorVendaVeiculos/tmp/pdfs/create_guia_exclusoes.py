from pathlib import Path
from xml.sax.saxutils import escape

from reportlab.lib import colors
from reportlab.lib.enums import TA_CENTER, TA_LEFT
from reportlab.lib.pagesizes import A4
from reportlab.lib.styles import ParagraphStyle, getSampleStyleSheet
from reportlab.lib.units import cm
from reportlab.pdfbase import pdfmetrics
from reportlab.pdfbase.ttfonts import TTFont
from reportlab.platypus import (
    BaseDocTemplate,
    Frame,
    PageBreak,
    PageTemplate,
    Paragraph,
    Spacer,
    Table,
    TableStyle,
    XPreformatted,
)
from reportlab.platypus.tableofcontents import TableOfContents


ROOT = Path(__file__).resolve().parents[2]
OUTPUT = ROOT / "output" / "pdf" / "guia-exclusoes-entity-framework-core.pdf"

PAGE_W, PAGE_H = A4
MARGIN_X = 2.05 * cm
MARGIN_TOP = 2.15 * cm
MARGIN_BOTTOM = 1.75 * cm

INK = colors.HexColor("#1C2333")
INK_2 = colors.HexColor("#28304A")
SIGNAL = colors.HexColor("#E8871E")
STEEL = colors.HexColor("#64748B")
LINE = colors.HexColor("#E2E4E8")
PAPER = colors.HexColor("#FAFAF8")
PALE_ORANGE = colors.HexColor("#FFF3E6")
PALE_BLUE = colors.HexColor("#EEF4FF")
PALE_GREEN = colors.HexColor("#ECF8F1")
PALE_RED = colors.HexColor("#FFF0F0")
WHITE = colors.white


def register_fonts():
    candidates = {
        "Body": Path(r"C:\Windows\Fonts\arial.ttf"),
        "BodyBold": Path(r"C:\Windows\Fonts\arialbd.ttf"),
        "BodyItalic": Path(r"C:\Windows\Fonts\ariali.ttf"),
        "Mono": Path(r"C:\Windows\Fonts\consola.ttf"),
        "MonoBold": Path(r"C:\Windows\Fonts\consolab.ttf"),
    }
    fallback = {
        "Body": "Helvetica",
        "BodyBold": "Helvetica-Bold",
        "BodyItalic": "Helvetica-Oblique",
        "Mono": "Courier",
        "MonoBold": "Courier-Bold",
    }
    result = {}
    for name, path in candidates.items():
        if path.exists():
            pdfmetrics.registerFont(TTFont(name, str(path)))
            result[name] = name
        else:
            result[name] = fallback[name]
    return result


FONTS = register_fonts()


class GuideDocTemplate(BaseDocTemplate):
    def __init__(self, filename, **kwargs):
        super().__init__(filename, **kwargs)
        frame = Frame(
            MARGIN_X,
            MARGIN_BOTTOM,
            PAGE_W - (2 * MARGIN_X),
            PAGE_H - MARGIN_TOP - MARGIN_BOTTOM,
            id="main",
        )
        self.addPageTemplates(PageTemplate(id="guide", frames=frame, onPage=draw_page))

    def afterFlowable(self, flowable):
        if isinstance(flowable, Paragraph):
            style_name = flowable.style.name
            if style_name in ("H1", "H2"):
                level = 0 if style_name == "H1" else 1
                text = flowable.getPlainText()
                key = f"heading-{self.page}-{abs(hash(text))}"
                self.canv.bookmarkPage(key)
                if level == 0:
                    self.canv.addOutlineEntry(text, key, level=0, closed=False)
                self.notify("TOCEntry", (level, text, self.page, key))


def draw_page(canvas, doc):
    canvas.saveState()
    canvas.setFillColor(PAPER)
    canvas.rect(0, 0, PAGE_W, PAGE_H, stroke=0, fill=1)

    if doc.page == 1:
        canvas.setFillColor(INK)
        canvas.rect(0, PAGE_H - 8.4 * cm, PAGE_W, 8.4 * cm, stroke=0, fill=1)
        canvas.setFillColor(SIGNAL)
        canvas.rect(0, PAGE_H - 8.55 * cm, PAGE_W, 0.15 * cm, stroke=0, fill=1)
        canvas.setFillColor(SIGNAL)
        canvas.circle(PAGE_W - 2.2 * cm, PAGE_H - 2.2 * cm, 0.52 * cm, stroke=0, fill=1)
        canvas.setStrokeColor(WHITE)
        canvas.setLineWidth(2)
        canvas.circle(PAGE_W - 2.2 * cm, PAGE_H - 2.2 * cm, 0.25 * cm, stroke=1, fill=0)
    else:
        canvas.setStrokeColor(LINE)
        canvas.setLineWidth(0.7)
        canvas.line(MARGIN_X, PAGE_H - 1.35 * cm, PAGE_W - MARGIN_X, PAGE_H - 1.35 * cm)
        canvas.setFont(FONTS["BodyBold"], 8)
        canvas.setFillColor(INK)
        canvas.drawString(MARGIN_X, PAGE_H - 1.05 * cm, "STOCKCAR MANAGER")
        canvas.setFont(FONTS["Body"], 8)
        canvas.setFillColor(STEEL)
        canvas.drawRightString(
            PAGE_W - MARGIN_X,
            PAGE_H - 1.05 * cm,
            "Guia de exclusões e relacionamentos",
        )

        canvas.setStrokeColor(LINE)
        canvas.line(MARGIN_X, 1.22 * cm, PAGE_W - MARGIN_X, 1.22 * cm)
        canvas.setFont(FONTS["Body"], 8)
        canvas.setFillColor(STEEL)
        canvas.drawString(MARGIN_X, 0.83 * cm, "ASP.NET Core MVC + Entity Framework Core")
        canvas.drawRightString(PAGE_W - MARGIN_X, 0.83 * cm, f"Página {doc.page}")

    canvas.restoreState()


styles = getSampleStyleSheet()
styles.add(
    ParagraphStyle(
        name="CoverEyebrow",
        fontName=FONTS["BodyBold"],
        fontSize=10,
        leading=13,
        textColor=SIGNAL,
        spaceAfter=12,
        tracking=1.1,
    )
)
styles.add(
    ParagraphStyle(
        name="CoverTitle",
        fontName=FONTS["BodyBold"],
        fontSize=27,
        leading=32,
        textColor=WHITE,
        spaceAfter=15,
    )
)
styles.add(
    ParagraphStyle(
        name="CoverSubtitle",
        fontName=FONTS["Body"],
        fontSize=12,
        leading=18,
        textColor=colors.HexColor("#DDE3EE"),
    )
)
styles.add(
    ParagraphStyle(
        name="H1",
        fontName=FONTS["BodyBold"],
        fontSize=18,
        leading=23,
        textColor=INK,
        spaceBefore=8,
        spaceAfter=10,
        keepWithNext=True,
    )
)
styles.add(
    ParagraphStyle(
        name="H2",
        fontName=FONTS["BodyBold"],
        fontSize=13,
        leading=17,
        textColor=INK_2,
        spaceBefore=9,
        spaceAfter=6,
        keepWithNext=True,
    )
)
styles.add(
    ParagraphStyle(
        name="BodyTextCustom",
        fontName=FONTS["Body"],
        fontSize=9.6,
        leading=14.2,
        textColor=INK,
        spaceAfter=7,
        allowWidows=0,
        allowOrphans=0,
    )
)
styles.add(
    ParagraphStyle(
        name="BodySmall",
        parent=styles["BodyTextCustom"],
        fontSize=8.6,
        leading=12.5,
        textColor=STEEL,
    )
)
styles.add(
    ParagraphStyle(
        name="BulletCustom",
        parent=styles["BodyTextCustom"],
        leftIndent=14,
        firstLineIndent=-8,
        bulletIndent=0,
        spaceAfter=4,
    )
)
styles.add(
    ParagraphStyle(
        name="CodeCustom",
        fontName=FONTS["Mono"],
        fontSize=7.2,
        leading=10.2,
        textColor=colors.HexColor("#E8EDF5"),
        leftIndent=9,
        rightIndent=9,
        spaceBefore=5,
        spaceAfter=7,
        borderColor=INK,
        borderWidth=0,
        borderPadding=9,
        backColor=INK,
    )
)
styles.add(
    ParagraphStyle(
        name="CalloutText",
        parent=styles["BodyTextCustom"],
        fontSize=9.2,
        leading=13.5,
        spaceAfter=0,
    )
)
styles.add(
    ParagraphStyle(
        name="TOCHeading",
        fontName=FONTS["BodyBold"],
        fontSize=18,
        leading=22,
        textColor=INK,
        spaceAfter=14,
    )
)


def p(text, style="BodyTextCustom"):
    return Paragraph(text, styles[style])


def h1(text):
    return Paragraph(text, styles["H1"])


def h2(text):
    return Paragraph(text, styles["H2"])


def bullets(items):
    result = []
    for item in items:
        result.append(Paragraph(f"• {item}", styles["BulletCustom"]))
    return result


def code(text):
    return XPreformatted(escape(text.strip()), styles["CodeCustom"])


def callout(title, body, background=PALE_ORANGE, accent=SIGNAL):
    content = Paragraph(
        f'<font name="{FONTS["BodyBold"]}" color="{accent.hexval()}">{title}</font><br/>{body}',
        styles["CalloutText"],
    )
    table = Table([[content]], colWidths=[PAGE_W - 2 * MARGIN_X])
    table.setStyle(
        TableStyle(
            [
                ("BACKGROUND", (0, 0), (-1, -1), background),
                ("BOX", (0, 0), (-1, -1), 0.6, accent),
                ("LINEBEFORE", (0, 0), (0, -1), 4, accent),
                ("LEFTPADDING", (0, 0), (-1, -1), 12),
                ("RIGHTPADDING", (0, 0), (-1, -1), 12),
                ("TOPPADDING", (0, 0), (-1, -1), 9),
                ("BOTTOMPADDING", (0, 0), (-1, -1), 9),
            ]
        )
    )
    return table


def data_table(headers, rows, widths):
    body = [[p(f"<b>{escape(str(value))}</b>", "BodySmall") for value in headers]]
    for row in rows:
        body.append([p(escape(str(value)), "BodySmall") for value in row])
    table = Table(body, colWidths=widths, repeatRows=1, hAlign="LEFT")
    table.setStyle(
        TableStyle(
            [
                ("BACKGROUND", (0, 0), (-1, 0), INK),
                ("TEXTCOLOR", (0, 0), (-1, 0), WHITE),
                ("GRID", (0, 0), (-1, -1), 0.45, LINE),
                ("VALIGN", (0, 0), (-1, -1), "TOP"),
                ("LEFTPADDING", (0, 0), (-1, -1), 7),
                ("RIGHTPADDING", (0, 0), (-1, -1), 7),
                ("TOPPADDING", (0, 0), (-1, -1), 6),
                ("BOTTOMPADDING", (0, 0), (-1, -1), 6),
                ("ROWBACKGROUNDS", (0, 1), (-1, -1), [WHITE, colors.HexColor("#F5F6F8")]),
            ]
        )
    )
    return table


story = []

# Cover
story.append(Spacer(1, 0.65 * cm))
story.append(p("GUIA DIDÁTICO DE C# E EF CORE", "CoverEyebrow"))
story.append(p("Exclusões, relacionamentos e integridade dos dados", "CoverTitle"))
story.append(
    p(
        "Como decidir o que acontece com as vendas quando um veículo, cliente, marca ou cidade é excluído no StockCar Manager.",
        "CoverSubtitle",
    )
)
story.append(Spacer(1, 4.8 * cm))
story.append(
    callout(
        "Objetivo do material",
        "Explicar os conceitos antes da implementação, mostrar exemplos pequenos em C# e apresentar uma estratégia segura para preservar o histórico comercial da aplicação.",
        background=PALE_BLUE,
        accent=colors.HexColor("#3B6FB6"),
    )
)
story.append(Spacer(1, 0.5 * cm))
story.append(p("Projeto: StockCar Manager", "BodyTextCustom"))
story.append(p("Tecnologias: ASP.NET Core MVC, Entity Framework Core e PostgreSQL", "BodySmall"))
story.append(p("Material de apoio para iniciantes", "BodySmall"))
story.append(PageBreak())

# TOC
story.append(Paragraph("Sumário", styles["TOCHeading"]))
toc = TableOfContents()
toc.levelStyles = [
    ParagraphStyle(
        name="TOCLevel1",
        fontName=FONTS["BodyBold"],
        fontSize=10,
        leading=15,
        leftIndent=0,
        firstLineIndent=0,
        textColor=INK,
        spaceBefore=4,
    ),
    ParagraphStyle(
        name="TOCLevel2",
        fontName=FONTS["Body"],
        fontSize=8.8,
        leading=13,
        leftIndent=16,
        firstLineIndent=0,
        textColor=STEEL,
    ),
]
story.append(toc)
story.append(PageBreak())

story.append(h1("1. O problema que estamos resolvendo"))
story.append(
    p(
        "Em um banco relacional, uma venda não guarda todos os dados do veículo dentro dela. Em vez disso, a venda armazena o identificador do veículo, chamado de chave estrangeira. Essa referência permite saber exatamente qual veículo participou da negociação."
    )
)
story.append(code("""
public class Venda
{
    public int Id { get; private set; }

    public int VeiculoId { get; private set; }
    public Veiculo Veiculo { get; private set; }
}
"""))
story.append(
    callout(
        "Ideia principal",
        "<b>VeiculoId</b> guarda o número do veículo. <b>Veiculo</b> permite acessar o objeto relacionado no código C#. O banco garante que a referência continue válida.",
    )
)
story.append(h2("Relações existentes no projeto"))
story.extend(
    bullets(
        [
            "uma cidade pode estar relacionada a vários clientes;",
            "uma marca pode estar relacionada a vários veículos;",
            "um cliente pode estar relacionado a várias vendas;",
            "um veículo está referenciado por uma ou mais vendas no esquema atual.",
        ]
    )
)
story.append(Spacer(1, 4))
story.append(
    data_table(
        ["Registro principal", "Registro dependente", "Chave estrangeira"],
        [
            ["Cidade", "Cliente", "CidadeId"],
            ["Marca", "Veículo", "MarcaId"],
            ["Cliente", "Venda", "ClienteId"],
            ["Veículo", "Venda", "VeiculoId"],
        ],
        [4.2 * cm, 4.2 * cm, 5.1 * cm],
    )
)

story.append(h1("2. Vocabulário essencial"))
story.append(h2("Entidade"))
story.append(p("Uma classe C# que representa algo armazenado no banco, como Cidade, Cliente, Veiculo ou Venda."))
story.append(h2("Chave primária"))
story.append(p("Campo que identifica um registro de forma única. No projeto, esse campo normalmente é o Id."))
story.append(h2("Chave estrangeira"))
story.append(p("Campo que aponta para a chave primária de outra tabela. VeiculoId, por exemplo, aponta para o Id de Veiculo."))
story.append(h2("Entidade principal e dependente"))
story.append(
    p(
        "Na relação entre Veiculo e Venda, o veículo é o principal e a venda é a dependente, pois Venda precisa da chave do veículo para manter a associação."
    )
)
story.append(h2("Integridade referencial"))
story.append(
    p(
        "É a garantia de que o banco não manterá uma referência quebrada. Uma venda não pode apontar para um veículo que não existe, salvo quando o relacionamento for opcional e a chave puder ser nula."
    )
)
story.append(h2("Migration"))
story.append(
    p(
        "É uma descrição versionada das mudanças do banco. Quando o relacionamento muda no código, uma migration altera a regra correspondente no PostgreSQL."
    )
)

story.append(h1("3. Os comportamentos de exclusão"))
story.append(
    data_table(
        ["Comportamento", "O que acontece", "Uso típico"],
        [
            ["Cascade", "Exclui também os registros dependentes.", "Dados sem valor fora do registro principal."],
            ["Restrict", "Bloqueia a exclusão se houver dependentes.", "Histórico e dados comerciais."],
            ["NoAction", "Deixa o banco validar e bloquear a operação.", "Semelhante a Restrict no PostgreSQL."],
            ["SetNull", "Mantém o dependente e limpa a chave estrangeira.", "Relacionamentos realmente opcionais."],
            ["Exclusão lógica", "Mantém a linha e marca o registro como inativo.", "Auditoria e recuperação futura."],
        ],
        [3.0 * cm, 5.1 * cm, 5.4 * cm],
    )
)
story.append(Spacer(1, 10))
story.append(
    callout(
        "Pergunta que orienta a decisão",
        "Se o registro principal desaparecer, o dependente perde completamente o sentido? Se a resposta for não, apagar em cascata provavelmente não é a melhor escolha.",
        background=PALE_BLUE,
        accent=colors.HexColor("#3B6FB6"),
    )
)

story.append(h1("4. Opção A: exclusão em cascata"))
story.append(
    p(
        "Com Cascade, apagar o veículo também apaga as vendas que possuem aquele VeiculoId. O PostgreSQL realiza essa operação por causa da regra gravada na chave estrangeira."
    )
)
story.append(code("""
modelBuilder.Entity<Venda>()
    .HasOne(venda => venda.Veiculo)
    .WithMany()
    .HasForeignKey(venda => venda.VeiculoId)
    .OnDelete(DeleteBehavior.Cascade);
"""))
story.append(h2("Lendo o código linha por linha"))
story.extend(
    bullets(
        [
            "Entity&lt;Venda&gt; informa qual entidade está sendo configurada;",
            "HasOne informa que cada venda possui um veículo;",
            "WithMany informa que o lado principal pode ter vários dependentes;",
            "HasForeignKey seleciona VeiculoId como chave estrangeira;",
            "OnDelete escolhe o comportamento quando o veículo for excluído.",
        ]
    )
)
story.append(h2("Controller com Cascade"))
story.append(code("""
[HttpPost, ActionName("Delete")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> DeleteConfirmed(int id)
{
    var veiculo = await _context.Veiculos.FindAsync(id);

    if (veiculo != null)
    {
        _context.Veiculos.Remove(veiculo);
        await _context.SaveChangesAsync();
    }

    return RedirectToAction(nameof(Index));
}
"""))
story.append(
    p(
        "O controller não precisa carregar as vendas para que a cascata ocorra. A exclusão dos dependentes é responsabilidade da regra do banco."
    )
)
story.append(
    callout(
        "Risco para este projeto",
        "Excluir uma venda remove data, cliente, vendedor e valores usados no histórico e no dashboard. Por isso, Cascade é simples, mas perigoso para dados comerciais.",
        background=PALE_RED,
        accent=colors.HexColor("#B33A3A"),
    )
)

story.append(h1("5. Opção B: impedir a exclusão"))
story.append(
    p(
        "Com Restrict, um veículo sem vendas pode ser excluído. Quando existe uma venda relacionada, o banco recusa a operação. Essa é uma estratégia apropriada para preservar o histórico."
    )
)
story.append(code("""
modelBuilder.Entity<Venda>()
    .HasOne(venda => venda.Veiculo)
    .WithMany()
    .HasForeignKey(venda => venda.VeiculoId)
    .OnDelete(DeleteBehavior.Restrict);
"""))
story.append(h2("Validação amigável no controller"))
story.append(
    p(
        "Antes de mandar o DELETE ao banco, o controller pode verificar se há vendas. Isso permite mostrar uma mensagem compreensível ao usuário."
    )
)
story.append(code("""
var possuiVendas = await _context.Vendas
    .AnyAsync(venda => venda.VeiculoId == id);

if (possuiVendas)
{
    TempData["Erro"] =
        "O veículo possui uma venda e não pode ser excluído.";

    return RedirectToAction(nameof(Index));
}
"""))
story.append(
    p(
        "AnyAsync não traz todas as vendas para a memória. A consulta apenas pergunta ao banco se existe pelo menos um registro correspondente."
    )
)
story.append(h2("Exclusão depois da validação"))
story.append(code("""
var veiculo = await _context.Veiculos.FindAsync(id);

if (veiculo != null)
{
    _context.Veiculos.Remove(veiculo);
    await _context.SaveChangesAsync();
}
"""))
story.append(
    callout(
        "Duas camadas de proteção",
        "O controller oferece uma boa mensagem. A restrição no banco oferece a garantia final, inclusive quando duas pessoas usam o sistema ao mesmo tempo.",
        background=PALE_GREEN,
        accent=colors.HexColor("#247A4A"),
    )
)

story.append(h1("6. Tratando erros do banco"))
story.append(
    p(
        "Mesmo depois da consulta com AnyAsync, outro usuário pode registrar uma venda antes da exclusão terminar. Esse cenário é chamado de condição de corrida. O banco ainda precisa proteger os dados."
    )
)
story.append(code("""
try
{
    _context.Veiculos.Remove(veiculo);
    await _context.SaveChangesAsync();
}
catch (DbUpdateException)
{
    TempData["Erro"] =
        "Não foi possível excluir o veículo porque ele está em uso.";

    return RedirectToAction(nameof(Index));
}
"""))
story.append(
    p(
        "DbUpdateException representa uma falha ocorrida enquanto o Entity Framework tentava salvar mudanças. Em uma aplicação maior, o erro também deve ser registrado em log."
    )
)

story.append(h1("7. Opção C: exclusão manual dos dependentes"))
story.append(
    p(
        "Outra possibilidade é carregar as vendas, marcar todas para exclusão e depois remover o veículo. Isso oferece controle explícito, mas continua apagando o histórico."
    )
)
story.append(code("""
var veiculo = await _context.Veiculos.FindAsync(id);

var vendas = await _context.Vendas
    .Where(venda => venda.VeiculoId == id)
    .ToListAsync();

_context.Vendas.RemoveRange(vendas);
_context.Veiculos.Remove(veiculo);

await _context.SaveChangesAsync();
"""))
story.append(h2("Por que usar um único SaveChangesAsync?"))
story.append(
    p(
        "O Entity Framework normalmente agrupa essas mudanças em uma transação. Se uma etapa falhar, o banco pode desfazer toda a operação e evitar que apenas uma parte dos dados seja apagada."
    )
)
story.append(
    callout(
        "Quando faz sentido",
        "Use a exclusão manual quando os dependentes realmente precisam ser apagados e você deseja executar ações adicionais, como gerar logs ou remover arquivos relacionados.",
        background=PALE_BLUE,
        accent=colors.HexColor("#3B6FB6"),
    )
)

story.append(h1("8. Opção D: SetNull"))
story.append(
    p(
        "SetNull mantém a venda, mas define VeiculoId como nulo. Para isso, a propriedade precisa aceitar null. O ponto de interrogação transforma int e Veiculo em tipos anuláveis."
    )
)
story.append(code("""
public int? VeiculoId { get; private set; }
public Veiculo? Veiculo { get; private set; }
"""))
story.append(code("""
modelBuilder.Entity<Venda>()
    .HasOne(venda => venda.Veiculo)
    .WithMany()
    .HasForeignKey(venda => venda.VeiculoId)
    .OnDelete(DeleteBehavior.SetNull);
"""))
story.append(
    p(
        "Essa opção não combina muito bem com Venda, porque uma venda sem veículo perde uma informação central. Ela é mais adequada para relações que sejam verdadeiramente opcionais."
    )
)

story.append(h1("9. Opção E: exclusão lógica"))
story.append(
    p(
        "Na exclusão lógica, a linha continua no banco. O sistema apenas marca o veículo como inativo. Essa estratégia preserva o histórico e permite recuperação futura."
    )
)
story.append(code("""
public class Veiculo
{
    public bool Ativo { get; private set; } = true;

    public void Desativar()
    {
        Ativo = false;
    }
}
"""))
story.append(h2("Controller"))
story.append(code("""
var veiculo = await _context.Veiculos.FindAsync(id);

if (veiculo != null)
{
    veiculo.Desativar();
    await _context.SaveChangesAsync();
}
"""))
story.append(h2("Filtro global"))
story.append(code("""
modelBuilder.Entity<Veiculo>()
    .HasQueryFilter(veiculo => veiculo.Ativo);
"""))
story.append(
    p(
        "Depois do filtro global, consultas normais retornam somente veículos ativos. Para uma tela administrativa que também mostre os inativos, pode-se usar IgnoreQueryFilters."
    )
)
story.append(code("""
var todos = await _context.Veiculos
    .IgnoreQueryFilters()
    .ToListAsync();
"""))

story.append(h1("10. Estratégia recomendada para o StockCar Manager"))
story.append(
    data_table(
        ["Situação", "Comportamento recomendado"],
        [
            ["Veículo nunca vendido", "Permitir exclusão física."],
            ["Veículo com venda", "Bloquear exclusão ou desativar."],
            ["Veículo fora do estoque", "Desativar e preservar o histórico."],
            ["Venda digitada incorretamente", "Corrigir ou cancelar de forma controlada."],
            ["Venda concluída", "Preservar como registro histórico."],
            ["Marca com veículos", "Bloquear a exclusão da marca."],
            ["Cidade com clientes", "Bloquear a exclusão da cidade."],
            ["Cliente com vendas", "Bloquear ou desativar o cliente."],
        ],
        [5.4 * cm, 8.1 * cm],
    )
)
story.append(Spacer(1, 10))
story.append(
    callout(
        "Recomendação prática",
        "Use <b>Restrict</b> nos relacionamentos que envolvem vendas. Considere exclusão lógica para veículos e clientes. Preserve vendas concluídas, pois elas alimentam o histórico e os indicadores do sistema.",
        background=PALE_GREEN,
        accent=colors.HexColor("#247A4A"),
    )
)

story.append(h1("11. Configurando todos os relacionamentos"))
story.append(code("""
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<Cliente>()
        .HasOne(cliente => cliente.Cidade)
        .WithMany()
        .HasForeignKey(cliente => cliente.CidadeId)
        .OnDelete(DeleteBehavior.Restrict);

    modelBuilder.Entity<Veiculo>()
        .HasOne(veiculo => veiculo.Marca)
        .WithMany()
        .HasForeignKey(veiculo => veiculo.MarcaId)
        .OnDelete(DeleteBehavior.Restrict);

    modelBuilder.Entity<Venda>()
        .HasOne(venda => venda.Cliente)
        .WithMany()
        .HasForeignKey(venda => venda.ClienteId)
        .OnDelete(DeleteBehavior.Restrict);

    modelBuilder.Entity<Venda>()
        .HasOne(venda => venda.Veiculo)
        .WithMany()
        .HasForeignKey(venda => venda.VeiculoId)
        .OnDelete(DeleteBehavior.Restrict);
}
"""))
story.append(
    p(
        "A configuração deve ficar no método OnModelCreating da classe ApplicationDbContext. Ela substitui o comportamento que o Entity Framework escolheria apenas por convenção."
    )
)

story.append(PageBreak())
story.append(h1("12. Atualizando o banco com migrations"))
story.append(
    p(
        "Modificar o código do ApplicationDbContext não altera sozinho um banco já criado. É preciso gerar e aplicar uma migration."
    )
)
story.append(h2("Passo 1: modificar o relacionamento"))
story.append(p("Adicione o OnDelete desejado dentro de OnModelCreating e salve o arquivo."))
story.append(h2("Passo 2: criar a migration"))
story.append(code("""
dotnet ef migrations add RestringirExclusoes \
  --project GerenciadorVendaVeiculos/GerenciadorVendaVeiculos.csproj
"""))
story.append(h2("Passo 3: revisar a migration"))
story.append(
    p(
        "Abra o arquivo gerado e confira se a chave estrangeira foi recriada com o comportamento esperado. Esse passo ajuda a detectar alterações não planejadas."
    )
)
story.append(h2("Passo 4: aplicar no PostgreSQL"))
story.append(code("""
dotnet ef database update \
  --project GerenciadorVendaVeiculos/GerenciadorVendaVeiculos.csproj
"""))
story.append(h2("Passo 5: testar"))
story.extend(
    bullets(
        [
            "crie um veículo sem venda e confirme que ele pode ser excluído;",
            "crie outro veículo e registre uma venda;",
            "tente excluir o veículo vendido e confira se a operação é bloqueada;",
            "confirme que a venda continua no banco;",
            "confira se a mensagem apresentada é compreensível.",
        ]
    )
)
story.append(
    callout(
        "Antes de migrations destrutivas",
        "Faça backup quando houver dados importantes. Leia a migration gerada e nunca teste uma mudança de exclusão pela primeira vez diretamente em produção.",
        background=PALE_RED,
        accent=colors.HexColor("#B33A3A"),
    )
)

story.append(h1("13. Mostrando mensagens na interface"))
story.append(
    p(
        "TempData transporta uma mensagem durante o redirecionamento. O controller define a mensagem e a view seguinte a exibe."
    )
)
story.append(code("""
TempData["Erro"] =
    "O veículo possui uma venda e não pode ser excluído.";

return RedirectToAction(nameof(Index));
"""))
story.append(h2("Exemplo na Razor View"))
story.append(code("""
@if (TempData["Erro"] is string mensagem)
{
    <div class="rounded-md bg-red-50 p-4 text-red-700">
        @mensagem
    </div>
}
"""))
story.append(
    p(
        "A mensagem existe somente até ser lida. Isso combina com o padrão Post/Redirect/Get usado depois de formulários."
    )
)

story.append(PageBreak())
story.append(h1("14. Como pensar antes de escolher Cascade"))
story.extend(
    bullets(
        [
            "O registro dependente possui valor histórico, fiscal ou de auditoria?",
            "O dashboard ou algum relatório depende desse registro?",
            "A exclusão poderia atravessar vários relacionamentos e apagar mais do que o esperado?",
            "O usuário entende claramente o impacto antes de confirmar?",
            "Existe backup ou possibilidade de recuperação?",
            "Uma desativação atenderia melhor à necessidade?",
        ]
    )
)
story.append(h2("Exemplo de efeito em cadeia"))
story.append(code("""
Excluir Marca
    -> exclui Veículos da marca
        -> exclui Vendas dos veículos
            -> altera totais e apaga histórico
"""))
story.append(
    p(
        "Mesmo que cada relacionamento pareça simples isoladamente, uma sequência de cascatas pode produzir uma exclusão ampla. Por isso, entidades cadastrais e transacionais merecem políticas diferentes."
    )
)

story.append(h1("15. Checklist de implementação"))
story.extend(
    bullets(
        [
            "definir por escrito o comportamento esperado para cada relacionamento;",
            "configurar DeleteBehavior no ApplicationDbContext;",
            "criar e revisar a migration;",
            "aplicar a migration em um banco de desenvolvimento;",
            "consultar dependências no controller para gerar mensagens amigáveis;",
            "tratar DbUpdateException como proteção adicional;",
            "mostrar o erro na Razor View;",
            "testar exclusão com e sem dependentes;",
            "testar operações simultâneas quando a regra for crítica;",
            "adicionar testes automatizados para evitar regressões.",
        ]
    )
)

story.append(h1("16. Exemplos de cenários de teste"))
story.append(
    data_table(
        ["Cenário", "Ação", "Resultado esperado"],
        [
            ["Veículo sem venda", "Excluir veículo", "Veículo removido."],
            ["Veículo vendido", "Excluir veículo", "Operação bloqueada e venda preservada."],
            ["Marca com veículos", "Excluir marca", "Operação bloqueada."],
            ["Cidade com clientes", "Excluir cidade", "Operação bloqueada."],
            ["Cliente com vendas", "Excluir cliente", "Operação bloqueada."],
            ["Registro inexistente", "Excluir por Id inválido", "Retorno seguro, sem falha geral."],
            ["Conflito simultâneo", "Vender e excluir ao mesmo tempo", "Banco preserva a integridade."],
        ],
        [3.7 * cm, 4.0 * cm, 5.8 * cm],
    )
)

story.append(PageBreak())
story.append(h1("17. Resumo final"))
story.append(
    data_table(
        ["Escolha", "Resumo"],
        [
            ["Cascade", "Apaga dependentes automaticamente. Simples, mas pode remover histórico."],
            ["Restrict", "Impede exclusões que quebrariam relações. Recomendado para vendas."],
            ["NoAction", "Deixa a restrição ser aplicada pelo banco."],
            ["SetNull", "Mantém o dependente sem o principal. Exige chave estrangeira anulável."],
            ["Exclusão manual", "O código controla a ordem e os registros removidos."],
            ["Exclusão lógica", "Preserva a linha e marca o registro como inativo."],
        ],
        [3.4 * cm, 10.1 * cm],
    )
)
story.append(Spacer(1, 12))
story.append(
    callout(
        "Conclusão",
        "Para o StockCar Manager, o caminho mais seguro é preservar as vendas, restringir a exclusão de registros usados por elas e utilizar desativação quando algo não deve mais aparecer na operação diária.",
        background=PALE_GREEN,
        accent=colors.HexColor("#247A4A"),
    )
)
story.append(Spacer(1, 16))
story.append(p("Próximo passo sugerido", "H2"))
story.append(
    p(
        "Implementar primeiro a restrição Veiculo -> Venda em uma branch de estudo, criar a migration, testar os dois cenários principais e somente depois repetir o padrão para Cliente, Marca e Cidade."
    )
)


OUTPUT.parent.mkdir(parents=True, exist_ok=True)
doc = GuideDocTemplate(
    str(OUTPUT),
    pagesize=A4,
    rightMargin=MARGIN_X,
    leftMargin=MARGIN_X,
    topMargin=MARGIN_TOP,
    bottomMargin=MARGIN_BOTTOM,
    title="Exclusões, relacionamentos e integridade dos dados",
    author="StockCar Manager",
    subject="Guia didático de C# e Entity Framework Core",
)
doc.multiBuild(story)
print(OUTPUT)
