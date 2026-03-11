# Nuke Project Utilities

Projetos modernos exigem inicialização rápida. Funcionalidades básicas devem ser organizadas em blocos modulares e reutilizáveis, evitando a recriação do zero.

### Algorithms

#### Cryptography

##### Argon2id

> O problema dos algoritmos de hash tradicionais (como bcrypt) é a dependência exclusiva do custo de CPU. Isso os torna vulneráveis ao poder massivo de processamento paralelo das GPUs modernas. O [Argon2](https://password-hashing.net/) resolve essa falha arquitetural introduzindo a restrição de memória (Memory-Hard). Ao exigir alocação pesada de RAM para o cálculo do hash, ele neutraliza a vantagem de hardware do atacante, tornando o custo do ataque computacionalmente inviável.
> 
> 
> 
> O algoritmo possui três variantes: Argon2i (otimizado contra ataques side-channel), Argon2d (otimizado contra ataques de GPU) e Argon2id, a versão híbrida recomendada para o hash de senhas.
> 
> #### Parâmetros de Configuração
> 
> A eficácia do algoritmo depende do ajuste arquitetural de seus parâmetros estruturais:
> 
> * **Salt Size**: Tamanho do valor aleatório anexado à senha para invalidar ataques de dicionário e *rainbow tables*.
> * **Pepper**: Chave secreta global aplicada à senha, armazenada isoladamente do banco de dados (exemplo: Azure Key Vault). Protege os hashes em caso de vazamento completo da base de dados.
> * **Hash Size**: Comprimento final em bytes da saída gerada.
> * **Memory Cost**: Quantidade de memória RAM exigida para a computação, criando o gargalo que inviabiliza ataques massivos em GPUs.
> * **Parallelism**: Número de *threads* independentes executadas simultaneamente, ajustando o consumo à arquitetura de concorrência do servidor.
> * **Time Cost**: Número de iterações de processamento, definindo o custo de CPU estrito para resolver o cálculo de cada hash.
> 
> #### Implementação no .NET
> 
> A implementação deve utilizar o [Padrão Options (`IOptions<T>`)](https://learn.microsoft.com/en-us/dotnet/core/extensions/options) da Microsoft para vincular as configurações a objetos de tipagem forte.
> 
> Definição estrutural no arquivo `appsettings.json`:
> 
> ```json
> "Argon2IdOptions": {
>   "Pepper": "j95mpmza@95u3^yb",
>   "SaltSize": 3,
>   "HashSize": 9,
>   "Parallelism": 2,
>   "TimeCost": 4,
>   "MemoryCost": 256000
> }
> ```
> 
> O vínculo no momento de inicialização utiliza o provedor de [Configuração no .NET](https://learn.microsoft.com/en-us/dotnet/core/extensions/configuration) no arquivo `Program.cs`:
> 
> ```csharp
> builder.Services.Configure<Argon2IdOptions>(builder.Configuration.GetSection("Argon2IdOptions"));
> ```
> 
> Para dados sensíveis como o `Pepper`, é arquiteturalmente incorreto o armazenamento em arquivos físicos no ambiente de produção. O [Gerenciamento Seguro de Segredos de Aplicativos](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) deve ser aplicado utilizando variáveis de ambiente. O provedor do .NET sobrescreve os dados lendo variáveis estruturadas pelo prefixo `__` (`Argon2IdOptions__Pepper`).
> 
> ```csharp
> var pepper = builder.Configuration["Argon2IdOptions:Pepper"];
> ```
> 
> O serviço criptográfico deve ser encapsulado e registrado no contêiner de [Injeção de Dependência do .NET](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection) para o consumo em outras camadas da aplicação:
> 
> ```csharp
> builder.Services.AddSingleton<Argon2IdService>();
> ```

  
#### GraphSearch

> ##### AStarSearch
> > efqwefqfq
> ##### BellmanFordSearch
> > qwerfqfqf
> ##### BreadthFirstSearch
> > eqfqf
> ##### DijkstraSearch
> > weggerw
> ##### DuanGuRenSearch
> > egfwegweg
     
#### Sort

> ##### HeapSort
> > egfwegweg
> ##### InsertionSort
> > egfwegweg
> ##### IntrospectiveSort
> > egfwegweg
> ##### QuickSort
> > egfwegweg
> ##### StableSort
> > egfwegweg

### Data structures
### ExtensionMethods
### Patterns
### Services
