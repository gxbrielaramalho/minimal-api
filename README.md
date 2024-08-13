# API Mínima com .NET

## Sobre

Este projeto é uma API mínima desenvolvida com .NET 8.0. O objetivo é fornecer uma aplicação simples que exemplifica o uso básico de ASP.NET Core e Entity Framework para a criação de uma API.

## Tecnologias Utilizadas

- **.NET 8.0**: Framework de desenvolvimento utilizado para construir a aplicação.
- **ASP.NET Core**: Framework para construção da API.
- **Entity Framework Core**: ORM para interagir com o banco de dados.
- **BCrypt.Net-Next**: Biblioteca para criptografia de senhas.

## Estrutura do Projeto

O projeto é estruturado da seguinte maneira:

- **Models**: Contém as classes de modelo que representam as entidades do banco de dados.
  - `Veiculo.cs`: Representa um veículo com propriedades `Marca` e `Modelo`.
  - `Administrador.cs`: Representa um administrador com propriedades `Username`, `PasswordHash`, e `Role`.

- **Data**: Contém a configuração do contexto do banco de dados.
  - `AppDbContext.cs`: Configura o Entity Framework Core e define as DbSets para as entidades.

- **Program.cs**: Configura e inicializa a API, incluindo a configuração do banco de dados e a configuração dos endpoints.

## Como Executar

## Clone o Repositório

Primeiro, clone o repositório para sua máquina local:

git clone https://github.com/gxbrielaramalho/minimal-api.git

## Navegue para o Diretório do Projeto

Entre no diretório do projeto:

cd minimal-api

## Restaurar Pacotes

Restaure os pacotes NuGet necessários:

dotnet restore

## Executar Migrations

Caso haja alterações no modelo de dados, execute as migrations:

dotnet ef migrations add InitialCreate

dotnet ef database update

## Executar a API

Finalmente, execute a aplicação:

dotnet run

A API estará disponível em http://localhost:5000.

## Como Contribuir
Se você deseja contribuir para este projeto, sinta-se à vontade para abrir um pull request ou reportar problemas.

## Notas
Problemas de Compilação: Se você encontrar problemas de compilação relacionados a tipos ou pacotes ausentes, verifique se todos os pacotes NuGet necessários estão instalados e se as diretivas using estão corretamente configuradas.
Erros de Propriedade: Caso veja avisos sobre propriedades não anuláveis, considere inicializar as propriedades no construtor ou declará-las como anuláveis se apropriado.
