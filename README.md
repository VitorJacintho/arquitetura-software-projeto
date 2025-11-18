# Sitema de biblioteca Escolar

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

