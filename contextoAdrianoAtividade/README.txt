Projeto Faculdade: DFS e BFS - Jogo de Palavras

Scene/Sample Scene

Este projeto é uma aplicação em Unity que demonstra a aplicação dos algoritmos de busca em largura (BFS - Breadth-First Search) e busca em profundidade (DFS - Depth-First Search) no contexto de um jogo de "ligação de palavras".

O código-fonte principal está escrito em C# e é executado dentro do ambiente de desenvolvimento Unity.

Estrutura do Projeto

O projeto utiliza dois scripts principais em C# localizados em Assets/_Scritps/:

1.
WordBank.cs:

• Define uma lista de palavras (palavras).

• Define uma palavra correta/alvo (palavraCorreta).

• Gera um grafo não-direcionado onde os nós são as palavras.

• A conexão entre duas palavras é estabelecida se a Distância de Levenshtein entre elas estiver entre as 3 menores distâncias para cada palavra (o que as torna "vizinhos" no grafo).

• Contém a implementação do algoritmo BFS (CalcularDistancia) para encontrar a menor distância (em número de palavras) entre duas palavras no grafo.

• Contém a implementação da Distância de Levenshtein (CalcularDistanciaLevenshtein) para medir a similaridade entre as palavras.



2.
IATextWriter.cs:

• Utiliza o grafo gerado em WordBank.cs.

• Implementa o algoritmo BFS (EncontrarCaminho) para encontrar o caminho mais curto (sequência de palavras) entre uma palavra inicial válida e a palavraCorreta.

• A palavra inicial é escolhida aleatoriamente, mas deve ter um caminho até a palavra correta.
 
• Simula a "digitação" das palavras do caminho encontrado na interface do usuário, demonstrando a sequência de tentativas até chegar à palavra correta.



Configurar o WordBank:

• No painel Hierarchy, localize o objeto de jogo que contém o script WordBank.cs (provavelmente um objeto chamado "GameManager" ou similar).

• No painel Inspector, você pode alterar a Palavra Secreta (palavraCorreta) para testar diferentes alvos. A lista de palavras (palavras) também pode ser editada.



3.
Configurar o IATextWriter:

• Localize o objeto de jogo que contém o script IATextWriter.cs.

• No painel Inspector, você pode ajustar as configurações de simulação:

• Intervalo Entre Palavras: Tempo de espera entre a exibição de uma palavra e a próxima.

• Velocidade Digitação: Velocidade com que cada letra da palavra é exibida.








