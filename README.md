# ProcessamentoEmLote

Projeto desenvolvido em **.NET 10** para processamento em lote de dados de clubes e jogadores de futebol.

O sistema lê um arquivo de entrada no formato **JSONL** (ou **JSON**), aplica regras de negócio e gera dois arquivos CSV:

- `clubs.csv` → contém um registro por clube.
- `players.csv` → contém um registro por jogador.

---

# Como Executar

## Pré-requisitos

- .NET 10 SDK instalado.
- Arquivo de entrada nos formatos `.jsonl` ou `.json`.

Download do SDK:

https://dotnet.microsoft.com/download

---

## Passos

### 1. Clonar o repositório

```bash
git clone https://github.com/seuusuario/ProcessamentoEmLote.git
cd ProcessamentoEmLote
```

### 2. Adicionar o arquivo de entrada

Coloque o arquivo `.jsonl` ou `.json` na pasta:

```text
Data/Input/
```

### 3. Executar o projeto

```bash
dotnet run
```

### 4. Verificar os arquivos gerados

Os arquivos CSV serão criados em:

```text
Data/Output/
```

---

# Estrutura do Projeto

```text
ProcessamentoEmLote/
│
├── Program.cs
├── README.md
│
├── Data/
│   ├── Input/        # Arquivos de entrada (.json/.jsonl)
│   └── Output/       # Arquivos gerados (.csv)
│
├── Models/
│   ├── Club.cs
│   └── Player.cs
│
├── Services/
│   ├── JsonReaderService.cs
│   ├── CsvWriterService.cs
│   └── ProcessingService.cs
│
└── Utils/
    └── Helpers e funções auxiliares
```

---

# Regras de Negócio

- Apenas clubes das séries **A** e **B** são processados.
- Cada jogador recebe o identificador (`club_id`) do clube ao qual pertence.
- Campos de lista (como cores) são convertidos para uma única string separada por `|` (pipe).
- Datas são exportadas no formato:

```text
yyyy-MM-dd
```

- Datas inválidas resultam em campo vazio.
- Campos ausentes ou nulos são exportados como campo vazio.
- Arquivos CSV são gerados em:
  - UTF-8
  - Com cabeçalho
  - Separados por vírgula
  - Compatíveis com RFC 4180
- Linhas inválidas são ignoradas sem interromper o processamento.
- Leitura e escrita são realizadas em streaming para suportar grandes volumes de dados.

---

# Dependências

## Opção 1 — Implementação Pura (.NET) -- CsvWriterService_s

Sem dependências externas.

Utiliza:

- `StreamReader`
- `StreamWriter`
- Manipulação manual de CSV

---

## Opção 2 — Utilizando CsvHelper -- CsvWriterService

Instalação:

```bash
dotnet add package CsvHelper
```

Benefícios:

- Escrita automática de CSV
- Configuração simplificada
- Melhor manutenção do código

Classes utilizadas:

- `CsvWriter`
- `CsvConfiguration`

---

# Tratamento de Erros

O sistema trata os seguintes cenários:

| Situação | Comportamento |
|-----------|--------------|
| Arquivo inexistente | Exibe erro informativo |
| Extensão inválida | Aceita apenas `.json` ou `.jsonl` |
| Arquivo vazio | Registra `[WARN]` e não gera saída |
| Linha inválida | Registra `[WARN]` e continua |
| Exceção inesperada | Registra `[ERROR]` sem encerrar abruptamente |

---

# Exemplo de Execução

### Entrada

```text
Data/Input/sample_clubes.jsonl
```

### Saídas Geradas

```text
Data/Output/clubs.csv
Data/Output/players.csv
```

---

# Resultado

Após a execução, serão gerados:

- `clubs.csv` contendo os dados dos clubes válidos.
- `players.csv` contendo os jogadores vinculados aos respectivos clubes.

O processamento é resiliente, escalável e preparado para trabalhar com grandes volumes de registros.
