# WebTasks - Projeto de Gerenciamento de Tarefas

## Descrição
O **WebTasks** é uma aplicação para gerenciamento de tarefas, construída com tecnologias modernas e boas práticas. O objetivo é permitir a gestão eficiente de tarefas com operações de CRUD dinâmicas e interface amigável, incluindo busca e modais para ações rápidas.

---

## **Stacks Utilizadas**
### **Frontend**
- **Bootstrap**: Para estilização e componentes responsivos.
- **DataTables**: Exibição e manipulação avançada de dados, com suporte a paginação, ordenação e busca.
- **JavaScript/jQuery**: Para funcionalidades dinâmicas e integração com o backend.

### **Backend**
- **ASP.NET Core**: Framework para desenvolvimento de APIs RESTful e Razor Pages.
- **Entity Framework Core**: Gerenciamento e mapeamento objeto-relacional (ORM) para o banco de dados.
- **SQL Server**: Banco de dados utilizado para armazenamento das informações.
- **dotenv.net**: Gerenciamento de variáveis de ambiente para maior segurança.

---

## **Funcionalidades**
- **CRUD Completo**: Operações para criar, ler, atualizar e excluir tarefas.
- **Modais Dinâmicos**: Modais para criação, edição e exclusão de tarefas, carregados via AJAX para uma experiência fluida.
- **Busca e Detalhes**: Busca por tarefas com exibição dos detalhes em uma página dedicada.
- **Validações**: Validações robustas no backend, com feedbacks claros ao usuário.
- **DataTables**: Integração para exibição de tarefas com busca, ordenação e paginação.

---

## **Rotas Utilizadas**
### **Rotas Estáticas**
Estas rotas são usadas para as páginas tradicionais de navegação:
- `GET /ToDoTasks/Index`: Lista todas as tarefas.
- `GET /ToDoTasks/Details/{id}`: Exibe os detalhes de uma tarefa específica.
- `GET /ToDoTasks/Create`: Exibe o formulário de criação de uma nova tarefa.
- `POST /ToDoTasks/Create`: Salva uma nova tarefa no banco de dados.
- `GET /ToDoTasks/Edit/{id}`: Exibe o formulário para edição de uma tarefa.
- `POST /ToDoTasks/Edit`: Atualiza uma tarefa existente.
- `GET /ToDoTasks/Delete/{id}`: Exibe a confirmação de exclusão de uma tarefa.
- `POST /ToDoTasks/DeleteConfirmed`: Remove a tarefa do banco de dados.

### **Rotas para Modais**
Estas rotas são exclusivas para carregar e manipular os modais dinamicamente:
- `GET /modals/ToDoTasksModal/Create`: Retorna o modal para criar uma nova tarefa.
- `POST /modals/ToDoTasksModal/Create`: Processa a criação da tarefa via AJAX.
- `GET /modals/ToDoTasksModal/Edit/{id}`: Retorna o modal para editar uma tarefa.
- `POST /modals/ToDoTasksModal/Edit`: Processa a edição da tarefa via AJAX.
- `GET /modals/ToDoTasksModal/Delete/{id}`: Retorna o modal para confirmar exclusão.
- `POST /modals/ToDoTasksModal/DeleteConfirmed`: Processa a exclusão da tarefa via AJAX.

---

## **Estrutura do Banco de Dados**

O projeto utiliza o **Entity Framework Core** para gerenciar o banco de dados. O banco de dados contém uma tabela principal chamada `tasks_db`.

### **Tabela: tasks_db**
| Campo          | Tipo               | Descrição                                    |
|----------------|--------------------|---------------------------------------------|
| `Id`           | `int`             | Chave primária, auto-incremento.            |
| `Title`        | `nvarchar(255)`   | Título da tarefa (obrigatório).             |
| `Description`  | `nvarchar(1000)`  | Descrição da tarefa (opcional).             |
| `TaskPriority` | `int`             | Prioridade da tarefa (enum: Low, Medium, High). |
| `Created`      | `datetime`        | Data de criação da tarefa (obrigatório).    |
| `IsCompleted`  | `bit`             | Status de conclusão (verdadeiro ou falso).  |

### **Enums Utilizados**
1. **Priority**
   - `Low = 0`
   - `Medium = 1`
   - `High = 2`

---

Confirme que o arquivo **`appsettings.json`** contém a configuração da string de conexão:
   ```json
   {
     "ConnectionStrings": {
       "WebTasksContext": "Server=SEU_SERVIDOR;Database=tasks_db;User Id=SEU_USUARIO;Password=SUA_SENHA;MultipleActiveResultSets=true"
     }
   }
   ```
---

## **Passo a Passo para Configurar o Banco de Dados**

### **1. Configuração do Ambiente**
1. Certifique-se de que o SQL Server está instalado e configurado.
2. Configure a string de conexão no arquivo `.env` conforme o exemplo acima.

---

### **2. Criando as Migrações**
1. No terminal do Visual Studio, execute o comando para criar as migrações:
   ```bash
   dotnet ef migrations add InitialCreate
   ```
2. Confirme que o diretório **`Migrations`** foi criado no projeto, contendo os arquivos da migração.

---

### **3. Aplicando as Migrações**
1. Execute o comando para aplicar as migrações ao banco de dados:
   ```bash
   dotnet ef database update
   ```
2. Verifique se a tabela **`tasks_db`** foi criada no banco de dados.

---

## **Rodando o Projeto**
1. Execute o comando no terminal do Visual Studio para rodar o projeto:
   ```bash
   dotnet run
   ```
2. Abra o navegador e acesse `http://localhost:5249`.
