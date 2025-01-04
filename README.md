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

```
-- cmd
$ routes.exe rotas.csv
```

```
-- Windows PowerShell
$ .\routes.exe rotas.csv
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