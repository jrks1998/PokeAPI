PokeAPI - teste técnico de desenvolvimento C#
Este projeto consome dados da PokeAPI para exibir e armazenar dados dos pokémons

Requisitos para execução do projeto:
1. Dotnet SDK 8.0 (https://dotnet.microsoft.com/pt-br/download/dotnet/thank-you/sdk-8.0.413-windows-x64-installer)
2. PostgreSQL (https://sbp.enterprisedb.com/getfile.jsp?fileid=1259680)
3. Postman (opcional, facilita envio das requisições, principalmente requisição POST) (https://dl.pstmn.io/download/latest/win64)

Para executar o projeto:
1. Fazer clone do projeto e acessar o diretório criado https://github.com/jrks1998/PokeAPI.git
2. Caso esteja usando cmd, executar ```definir_variaveis_ambiente_cmd.bat```
Caso esteja usando powershell, executar ```$env:DB_USER_POKEAPI = "usuario_banco"; $env:DB_PASS_POKEAPI = "senha_usuario_banco"; $env:DB_NAME_POKEAPI = "nome_banco"```
Substituir usuario_banco pelo nome do usuário, senha_usuario_banco pela senha e nomo_banco pelo nome do banco de dados
Esse projeto usa postgres como banco de dados. por padrão, o PostgreSQL roda na porta 5432. Caso o serviço esteja configurado para rodar em uma posta diferente dessa é possível Definir a variável de ambiente DB_PORT_POKEAPI para definir a porta usada para acessar o serviço do PostgreSQL
```$env:DB_PORT_POKEAPI = "porta"```
Substituir porta pelo número da porta
3. Executar ```dotnet clean```, após, ```dotnet restore``` para limpar, preparar e restaurar as ferramentas do projeto
4. Instalar o dotnet-ef para criar as tabelas ```dotnet tool install --global dotnet-ef```
5. Para criar as tabelas, executar ```dotnet ef database update```
6. Acessar o diretório PokeAPI e executar ```dotnet run```
7. Para executar os testes do controller, acessar o diretório pokeAPI.Tests e executar o código ```dotnet test```

Endpoints e exemplos:
1. Swagger disponível em http://localhost:5001/index.html
http://localhost:5001/swagger/v1/swagger.json
2. http://localhost:5001/pokemons/group-by-color
Exemplos de respostas

200 OK
```
{
  "green": [
    "bulbasaur",
    "ivysaur",
    "venusaur",
    "caterpie"
  ],
  "red": [
    "charmander",
    "charmeleon",
    "charizard"
  ],
  "blue": [
    "squirtle",
    "wartortle",
    "blastoise"
  ]
}
```

404 Not Found
```
Nenhum Pokemon encontrado
```
3. http://localhost:5001/pokemons/from-db
Exemplos de respostas

200 OK
```
{
  "green": [
    "Bulbasaur"
  ],
  "red": [
    "Charmander"
  ],
  "blue": [
    "Squirtle"
  ]
}
```

404 Not Found
```
Nenhum pokemon cadastrado no banco de dados
```
4. http://localhost:5001/pokemons/save
Exemplo de respostas
```
{
  "message": "Pokemons cadastrados com sucesso!",
  "groups": {
    "gold": [
      "pokemon dourado"
    ],
    "silver": [
      "pokemon prateado"
    ]
  }
}
```
