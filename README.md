# Sitema de biblioteca Escolar






1. Configuração de Inicialização

    - Precione com o Botão direito na Solução e acesse propriedades
    - selecione 'Varios Projetos de Inicialização:' e mude a ação de todos os projetos para Iniciar




2. Dependencias, baixar para cada projeto/microserviço (caso não instaladas devem ser baixadas)

    - Microsoft.EntityFrameworkCore (8.0.0)
    - Microsoft.EntityFrameworkCore.Design (8.0.0)
    - Microsoft.EntityFrameworkCore.Sqlite (8.0.0)
    - Microsoft.EntityFrameworkCore.Tools (8.0.0)
    - Swashbuckle.AspNetCore (6.5.0)



3. Gerar Banco de Dados

    - Na aba superior do Visual Studio 2022 acesse: Ferramentas -> Gerenciador de pacotes NuGet -> Console do Gerenciador de Pacotes

    - Para cada Projeto Execute os Seguintes Comandos

    ```Add-Migration InitialCreate```

    ```Update-Database```



4. Rodar projeto

    - Inicie o projeto precionando o botão 'Iniciar' no Visual Studio 2022


















# o que precisa ter
3 Microsserviços:

Alunos
Livros
Emprestimos

2 Buscas de dados entre microsserviços:

Livros: Mostra os livros e se esta emprestado.
Alunos: Mostra alunos e quantos livros cada aluno pegou emprestado.

1 microsserviço que altera dados de outro microsserviço:

Emprestimo altera quantidade de emprestimo de um aluno.

Banco de dados:

Alunos
id PK
nome STRING
qnt_emprestimo INT

Livro
id PK
titulo STRING
autor STRING
emprestado BOOLEAN

Emprestimo
id PK
data_emprestimo DATE
data_devolucao DATE
aluno FK
livro FK


Add-Migration InitialCreate

Update-Database


