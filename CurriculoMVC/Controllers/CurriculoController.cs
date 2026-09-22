using CurriculoMVC.DAO;
using CurriculoMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace CurriculoMVC.Controllers;

public class CurriculoController : Controller
{
    private readonly CurriculoDAO _dao;

    public CurriculoController(CurriculoDAO dao)
    {
        _dao = dao;
    }

    // GET /Curriculo  -> menu principal com a lista (CPF e Nome)
    public IActionResult Index()
    {
        return View(_dao.Listar());
    }

    // GET /Curriculo/Details/5  -> currículo formatado
    public IActionResult Details(int id)
    {
        var curriculo = _dao.BuscarPorId(id);
        if (curriculo == null) return NotFound();
        return View(curriculo);
    }

    // GET /Curriculo/Create
    [HttpGet]
    public IActionResult Create()
    {
        var curriculo = new Curriculo();
        curriculo.Normalizar();
        return View(curriculo);
    }

    // POST /Curriculo/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Curriculo curriculo)
    {
        ValidarRegras(curriculo);
        if (!ModelState.IsValid) return View(curriculo);

        _dao.Inserir(curriculo);
        TempData["Mensagem"] = "Currículo cadastrado.";
        return RedirectToAction(nameof(Index));
    }

    // GET /Curriculo/Edit/5
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var curriculo = _dao.BuscarPorId(id);
        if (curriculo == null) return NotFound();

        // Mostra o CPF com máscara no formulário (ao salvar, volta a ser só dígitos)
        curriculo.Cpf = curriculo.CpfFormatado;
        return View(curriculo);
    }

    // POST /Curriculo/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Curriculo curriculo)
    {
        if (_dao.BuscarPorId(id) == null) return NotFound();

        curriculo.Id = id;
        ValidarRegras(curriculo);
        if (!ModelState.IsValid) return View(curriculo);

        _dao.Atualizar(curriculo);
        TempData["Mensagem"] = "Currículo atualizado.";
        return RedirectToAction(nameof(Index));
    }

    // GET /Curriculo/Delete/5  -> tela de confirmação
    [HttpGet]
    public IActionResult Delete(int id)
    {
        var curriculo = _dao.BuscarPorId(id);
        if (curriculo == null) return NotFound();
        return View(curriculo);
    }

    // POST /Curriculo/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        _dao.Excluir(id);
        TempData["Mensagem"] = "Currículo excluído.";
        return RedirectToAction(nameof(Index));
    }

    // ------------------------------------------------------------------
    // Regras que as anotações do Model não cobrem
    // ------------------------------------------------------------------
    private void ValidarRegras(Curriculo c)
    {
        c.Normalizar();

        // CPF: dígitos verificadores e unicidade
        if (!ModelState.TryGetValue(nameof(Curriculo.Cpf), out var estadoCpf) || estadoCpf.Errors.Count == 0)
        {
            if (!c.CpfValido())
                ModelState.AddModelError(nameof(Curriculo.Cpf), "CPF inválido.");
            else if (_dao.CpfExiste(c.Cpf, c.Id))
                ModelState.AddModelError(nameof(Curriculo.Cpf), "Já existe um currículo com este CPF.");
        }

        // Formação: a primeira é obrigatória; nas demais, registro começado precisa estar completo
        for (int i = 0; i < c.Formacoes.Count; i++)
        {
            var f = c.Formacoes[i];
            if (i == 0 && !f.Preenchida)
            {
                ModelState.AddModelError("Formacoes[0].Curso", "Informe ao menos uma formação ou curso.");
                continue;
            }
            if (!f.Preenchida) continue;

            if (string.IsNullOrWhiteSpace(f.Curso))
                ModelState.AddModelError($"Formacoes[{i}].Curso", "Informe o curso.");
            if (string.IsNullOrWhiteSpace(f.Instituicao))
                ModelState.AddModelError($"Formacoes[{i}].Instituicao", "Informe a instituição.");
        }

        for (int i = 0; i < c.Experiencias.Count; i++)
        {
            var e = c.Experiencias[i];
            if (!e.Preenchida) continue;

            if (string.IsNullOrWhiteSpace(e.Empresa))
                ModelState.AddModelError($"Experiencias[{i}].Empresa", "Informe a empresa.");
            if (string.IsNullOrWhiteSpace(e.Cargo))
                ModelState.AddModelError($"Experiencias[{i}].Cargo", "Informe o cargo.");
        }

        for (int i = 0; i < c.Idiomas.Count; i++)
        {
            var d = c.Idiomas[i];
            if (d.Preenchida && string.IsNullOrWhiteSpace(d.Nome))
                ModelState.AddModelError($"Idiomas[{i}].Nome", "Informe o idioma.");
        }
    }
}
