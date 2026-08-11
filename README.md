# ProcessamentoEmLote

Projeto em **.NET 10** para processamento em lote de dados de clubes e jogadores de futebol.

O sistema lê arquivos em formato **JSONL** ou **JSON**, filtra os clubes válidos, aplica regras de negócio e gera dois arquivos CSV:

- `clubs.csv` — registro de clubes.
- `players.csv` — registro de jogadores vinculados aos clubes processados.

---

# Requisitos

- .NET 10 SDK instalado.
- Arquivo de entrada no formato `.json` ou `.jsonl`.

Download do SDK:

https://dotnet.microsoft.com/download

---

# Como executar

## 1. Clonar o repositório

```bash
git clone https://github.com/wesleimoreira/ProcessamentoEmLote.git
cd ProcessamentoEmLote
```

## 2. Preparar o arquivo de entrada

Coloque o arquivo de entrada na pasta:

```text
ProcessamentoEmLote/Data/Input/
```

Exemplo:

```text
ProcessamentoEmLote/Data/Input/sample_clubes.jsonl
```

## 3. Executar o projeto

### Usando o arquivo padrão

```bash
dotnet run
```

### Usando um caminho específico

```bash
dotnet run --project ProcessamentoEmLote -- ProcessamentoEmLote/Data/Input/sample_clubes.jsonl
```

ou

```bash
dotnet run --project ProcessamentoEmLote -- C:\caminho\do\arquivo\entrada.jsonl
```

## 4. Verificar a saída

Os arquivos CSV serão gerados em:

```text
ProcessamentoEmLote/Data/Output/
```

---

# Estrutura do projeto

```text
ProcessamentoEmLote/
├── ProcessamentoEmLote/
│   ├── Program.cs
│   ├── ProcessamentoEmLote.csproj
│   ├── Data/
│   │   ├── Input/
│   │   └── Output/
│   ├── Models/
│   │   ├── Club.cs
│   │   └── Player.cs
│   ├── Services/
│   │   ├── JsonReaderService.cs
│   │   ├── CsvWriterService.cs
│   │   └── ProcessingService.cs
│   ├── DTOs/
│   │   ├── ClubCsv.cs
│   │   └── PlayerCsv.cs
│   └── Utils/
│       ├── DateUtils.cs
│       ├── Logger.cs
│       └── StringUtils.cs
├── ProcessamentoEmLote.Tests/
│   └── ... testes automatizados
├── README.md
├── ProcessamentoEmLote.slnx
└── .gitignore
```

---

# Regras de negócio

- Apenas clubes das séries **A** e **B** são processados.
- Cada jogador recebe o identificador do clube (`club_id`) no CSV de jogadores.
- Listas como `colors` são convertidas para uma única string separada por ` | `.
- Datas são exportadas no padrão:

```text
yyyy-MM-dd
```

- Datas inválidas são convertidas para campo vazio.
- Campo nulo ou ausente vira string vazia.
- Linhas inválidas são ignoradas sem interromper o processamento.
- Leitura e escrita são feitas em fluxo para reduzir memória e atender melhor a lotes grandes.
- Arquivos gerados em UTF-8 com cabeçalho e separador `,`.

---

# Tratamento de erros

O sistema registra e continua em cenários como:

- arquivo não encontrado;
- extensão inválida;
- arquivo vazio;
- linha com JSON inválido;
- exceções inesperadas durante a leitura ou exportação.

Exemplos de comportamento:

- `FileNotFoundException` → log de erro e interrupção do fluxo.
- `.json` ou `.jsonl` inválidos → exceção de tipo não suportado.
- linha vazia → ignorada.
- linha com JSON inválido → registrada como `[WARN]` e ignorada.

---

# Saídas geradas

A execução produz os seguintes arquivos:

```text
clubs.csv
players.csv
```

No diretório:

```text
ProcessamentoEmLote/Data/Output/
```

---

# Execução de testes

Para validar o projeto:

```bash
dotnet test
```

Resultado esperado:

- todos os testes da solução devem passar;
- o projeto deve compilar sem erros;
- o pipeline de leitura/exportação deve funcionar em cenários de sucesso e validação de erro.

---

# Observações de performance

O projeto foi implementado pensando em escalabilidade para grande volume de dados:

- leitura em streaming do JSON;
- escrita em fluxo para os arquivos CSV;
- processamento sem materializar a base completa em memória;
- exportação direta dos dados válidos somente após filtros de negócio.

---

# Resultado

Após a execução, o sistema gera:

- `clubs.csv` com os dados dos clubes válidos;
- `players.csv` com os jogadores associados aos clubes processados.

Esse fluxo é adequado para processamento em lote de dados estruturados e pode escalar melhor do que abordagens que carregam todo o conteúdo em memória antes de exportar.
