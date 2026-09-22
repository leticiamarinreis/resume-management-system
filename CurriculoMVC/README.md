# CurriculoMVC

Cadastro e exibição de currículos em ASP.NET Core MVC (.NET 8) com SQL Server.

## Como executar
1. Rode o script SQL que está no comentário no topo de `DAO/CurriculoDAO.cs`.
2. Ajuste `ConnectionStrings:DefaultConnection` em `appsettings.json` se não usar o LocalDB.
3. `dotnet restore` e `dotnet run` (ou F5 no Visual Studio).

## Estrutura
- `Models/Curriculo.cs` : Curriculo, Formacao, Experiencia, Idioma (validações e regras)
- `DAO/CurriculoDAO.cs` : acesso ao banco (ADO.NET) + script de criação da tabela
- `Controllers/CurriculoController.cs` : Index, Details, Create, Edit, Delete
- `Views/Curriculo/` : Index, Create, Edit, Delete, Details e `_Form` (parcial usada por Create e Edit)
- `Views/Shared/_Layout.cshtml`, `Views/_ViewImports.cshtml`, `Views/_ViewStart.cshtml` : exigidos pelo MVC
- `wwwroot/css/style.css` : estilos da aplicação e do currículo (com layout de impressão)
