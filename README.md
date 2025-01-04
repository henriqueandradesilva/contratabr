# ContrataBR - Rotas

Este projeto tem como objetivo desenvolver uma aplicação para gerenciamento de rotas. A aplicação permitirá cadastrar rotas e verificar o melhor custo para uma origem e destino.
Inspirado no repositório [Clean Architecture Manga](https://github.com/ivanpaulovich/clean-architecture-manga), o projeto adota uma arquitetura robusta e escalável, visando a criação de uma aplicação bem estruturada e fácil de manter. Através dessa abordagem, buscamos garantir um código de alta qualidade, com separação de preocupações e aplicação de boas práticas de design de software.

## Tecnologias Utilizadas

- **.NET 8 Web API**: Para construção da API.
- **PostgreSQL**: Banco de dados relacional para armazenamento de dados. (Com feature flag)
- **Swagger**: Documentação da API e versionamento.
- **Microsoft Extensions**: Para configuração e extensão da aplicação.

## Arquitetura

Este projeto adota princípios de **Clean Architecture**, **Onion Architecture** e **Hexagonal Architecture**, promovendo uma clara **Separação de Preocupações**. A estrutura é dividida em:

- **Domain**: Contém as entidades e regras de negócio.
- **Application**: Contém casos de uso e lógicas de aplicação.
- **Infrastructure**: Implementações concretas como acesso a dados.
- **User Interface**: Interface do usuário, onde a API é exposta aos clientes.

## Recursos

- **EF Core**: Para acesso a dados com um padrão de repositório.
- **Feature Flags**: Para ativação/desativação de funcionalidades.
- **Data Annotation**: Validação de dados nos modelos.

## Padrões e Práticas

- **SOLID**: Princípios de design que orientam a estruturação do código.
- **Unit Of Work**: Padrão para gerenciar transações.
- **Repository**: Padrão de projeto que abstrai a lógica de acesso a dados.
- **Use Case**: Organização das operações da aplicação.
- **Presenter Custom**: Lógica para formatação e apresentação dos dados.

## Testes

- **Unit Test**: Conjunto de testes automatizados que verificam o comportamento de unidades específicas da aplicação 
(como funções ou métodos) para garantir que elas funcionem corretamente de forma isolada.

## Coleção Postman

Esta coleção contém as requisições para a API. 

*Arquivo: [Coleção Postman](./documents/WebApi.postman_collection.json)*

## Padrão de Arquitetura

Aqui está uma visão geral da arquitetura do sistema.

![Padrão de Arquitetura](./documents/arquitetura.png)

##  Clone o repositório:
git clone https://github.com/henriqueandradesilva/contratabr.git

### Passos para Build e Execução Console pelo CMD ###
1. Execute o CMD e Navegue para o Diretório do Projeto

- Certifique-se de estar no diretório do projeto onde está o arquivo Routes.csproj. 
Por exemplo, suponha que o projeto esteja em D:\contratabr\route\src\Routes.

```
-- cmd
cd D:\contratabr\route\src\Routes
```

2. Build do Executável
- Use o comando abaixo para compilar o projeto e gerar o executável:

```
-- cmd
dotnet build -c Release
```

- Isso gerará o executável no diretório:

```
D:\contratabr\route\src\Routes\bin\Release\net8.0\routes.exe
```

3. Executar o Programa
- Depois de compilar, navegue até o diretório onde está o executável:

```
-- cmd
cd D:\contratabr\route\src\Routes\bin\Release\net8.0\
```

- E execute o programa passando o arquivo como argumento:

```
-- cmd
routes.exe rotas.csv
```

```
-- Windows PowerShell
.\routes.exe rotas.csv
```

# API de Rotas - Contrata BR

A API permite gerenciar rotas entre localidades, permitindo consultar a melhor rota em relação a custo e cadastrar novas rotas com origem, destino e valor.

## Base URL
```
https://localhost:61888
```

---

### **Configuração e Execução**
1. **Abra a solução no Visual Studio:**
Localize e abra o arquivo Routes Solution.sln.
2. **Defina o projeto docker-compose como o projeto de inicialização:** 
Clique com o botão direito no projeto WebApi no Gerenciador de Soluções e selecione "Definir como Projeto de Inicialização".
3. **Execute a aplicação:** 
Pressione F5 ou clique em "Iniciar" para executar a aplicação.
4. **Acesse a documentação Swagger:**
Abra o navegador e acesse: https://localhost:600100/swagger/index.html.

### **Consultar a Melhor Rota**
**GET** `/api/v1/rotas/melhor/origem/{origem}/destino/{destino}`

Retorna a melhor rota entre a origem e o destino fornecidos.

#### **Parâmetros**
- `origem` (obrigatório): Local de origem (string).
- `destino` (obrigatório): Local de destino (string).

#### **Exemplo de Resposta (200)**
```json
{
  "sucesso": true,
  "resultado": "GRU - BRC - SCL - ORL - CDG ao custo de $40",
  "listaNotificacao": []
}
```

#### **Códigos de Resposta**
- `200 OK`: Consulta realizada com sucesso.
- `400 Bad Request`: Requisição inválida.
- `404 Not Found`: Rota não encontrada.

---

### **Cadastrar Nova Rota**
**POST** `/api/v1/rotas`

Adiciona uma nova rota com origem, destino e valor.

#### **Body (JSON)**
```json
{
  "origem": "SCL",
  "destino": "BRC",
  "valor": 10.00
}
```

#### **Exemplo de Resposta (201)**
```json
{
  "sucesso": true,
  "resultado": {
      "id": 0,
       "dataCriacao": "2025-01-04T14:06:14.7926794Z",
       "dataAlteracao": null
  },
  "listaNotificacao": [
      {
          "mensagem": "Rota cadastrada com sucesso.",
           "tipoNotificacao": "success"
      }
  ]
}
```

#### **Códigos de Resposta**
- `201 Created`: Rota criada com sucesso.
- `400 Bad Request`: Dados inválidos na requisição.

---

## Esquemas de Dados

### **Objeto `GenericStringResponse`**
| Campo          | Tipo              | Descrição                    |
|----------------|-------------------|--------------------------------|
| `sucesso`          | boolean | Indica o sucesso da operação.        |
| `resultado`        | string  | Resultado da operação.               |
| `listaNotificacao` | array   | Lista de notificações.               |

### **Objeto `PostRouteRequest`**
| Campo   | Tipo              | Descrição                    |
|---------|-------------------|--------------------------------|
| `origem`| string            | Local de origem (obrigatório). |
| `destino`| string           | Local de destino (obrigatório).|
| `valor` | number (double)   | Valor da rota (obrigatório).  |

### **Objeto `GenericNotificationResponse`**
| Campo              | Tipo   | Descrição                                      |
|--------------------|--------|--------------------------------------------------|
| `mensagem`         | string | Mensagem descritiva.                            |
| `tipoNotificacao`  | string | Tipo de notificação (`success`, `warning`, `error`).|

---

## Contato
- **Nome:** Contrata BR
- **E-mail:** contato@contratabr.app

---

# Rota de Viagem #
Escolha a rota de viagem mais barata independente da quantidade de conexões. (OK)
Para isso precisamos inserir as rotas através de um arquivo de entrada. (OK)

## Arquivo de entrada ##
Formato: Origem,Destino,Valor (OK)

```rotas.csv
GRU,BRC,10
BRC,SCL,5
GRU,CDG,75
GRU,SCL,20
GRU,ORL,56
ORL,CDG,5
SCL,ORL,20
```

## Explicando ## 
Uma viajem de **GRU** para **CDG** existem as seguintes rotas:

1. GRU - BRC - SCL - ORL - CDG ao custo de $40
2. GRU - ORL - CDG ao custo de $61
3. GRU - CDG ao custo de $75
4. GRU - SCL - ORL - CDG ao custo de $45

- O melhor preço é da rota **1**, apesar de mais conexões, seu valor final é menor. (OK)
- O resultado da consulta no programa deve ser: **GRU - BRC - SCL - ORL - CDG ao custo de $40**. (OK)

### Execução do programa ###
A inicializacao do teste se dará por linha de comando onde o primeiro argumento é o arquivo de entrada. (OK)

``` cmd
$ executavel rotas.csv
```

### Projetos ###
Duas interfaces de consulta devem ser implementadas:

- Interface de console 
	O console deverá receber um input com a rota no formato "DE-PARA" e imprimir a melhor rota e valor. (OK)
  
  Exemplo:
  ```cmd
  Digite a rota: GRU-CGD
  Melhor Rota: GRU - BRC - SCL - ORL - CDG ao custo de $40
  Digite a rota: BRC-SCL
  Melhor Rota: BRC - SCL ao custo de $5
  ```

- Interface Rest
    A interface Rest deverá suportar 2 endpoints:
    - Registro de novas rotas. Essas novas rotas devem ser persistidas no arquivo csv utilizado como entrada(rotas.csv), (OK)
    - Consulta de melhor rota entre dois pontos. (OK)

## Entregáveis ##
* Envie apenas o código fonte (OK)
* Estruture sua aplicação seguindo as boas práticas de desenvolvimento (OK)
* Evite o uso de frameworks ou bibliotecas externas à linguagem (OK)
* Implemente testes unitários seguindo as boas práticas de mercado (OK)
* Em um arquivo Texto ou Markdown descreva: (OK)
  * Como executar a aplicação
  * Estrutura dos arquivos/pacotes
  * Explique as decisões de design adotadas para a solução
  * Descreva sua API Rest de forma simplificada