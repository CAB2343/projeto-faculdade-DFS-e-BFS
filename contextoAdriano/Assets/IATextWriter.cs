using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class IATextWriter : MonoBehaviour
{
    [Header("Referências")]
    public InputField outputField;
    public WordBank wordBank;

    [Header("Configurações")]
    public float intervaloEntrePalavras = 1.5f;
    public float velocidadeDigitacao = 0.05f;

    private Queue<string> filaTentativas = new Queue<string>();
    private bool rodando = false;

    void Start()
    {
        if (wordBank == null)
        {
            Debug.LogError("WordBank não atribuído.");
            return;
        }

        // Garante que começa com uma palavra que realmente pode chegar na palavra final
        string inicioValido = EncontrarInicioValido();
        if (inicioValido == null)
        {
            Debug.LogError("Nenhum ponto de partida no grafo leva até a palavra correta!");
            return;
        }

        List<string> caminho = EncontrarCaminho(inicioValido, wordBank.palavraCorreta);

        foreach (var palavra in caminho)
            filaTentativas.Enqueue(palavra);

        StartCoroutine(ProcessarTentativas());
    }

    IEnumerator ProcessarTentativas()
    {
        rodando = true;

        while (filaTentativas.Count > 0)
        {
            string tentativa = filaTentativas.Dequeue();
            float proximidade = wordBank.CalcularProximidade(tentativa);

            Debug.Log($"Tentando: {tentativa} → Proximidade: {proximidade:F2}%");

            yield return StartCoroutine(EscreverDevagar(tentativa));
            yield return new WaitForSeconds(intervaloEntrePalavras);

            if (tentativa == wordBank.palavraCorreta)
            {
                Debug.Log("Palavra correta encontrada!");
                break;
            }
        }

        rodando = false;
    }

    IEnumerator EscreverDevagar(string palavra)
    {
        if (outputField == null) yield break;

        outputField.text = "";
        foreach (char c in palavra)
        {
            outputField.text += c;
            yield return new WaitForSeconds(velocidadeDigitacao);
        }
    }

    /// <summary>
    /// Busca um ponto inicial aleatório que tenha caminho até a palavra final.
    /// </summary>
    string EncontrarInicioValido()
    {
        List<string> tentativas = new List<string>(wordBank.palavras);

        while (tentativas.Count > 0)
        {
            int i = Random.Range(0, tentativas.Count);
            string inicio = tentativas[i];
            tentativas.RemoveAt(i);

            if (wordBank.CalcularDistancia(inicio, wordBank.palavraCorreta) > 0)
                return inicio;
        }

        return null;
    }

    /// <summary>
    /// Usa BFS para encontrar o caminho mais curto entre duas palavras conectadas.
    /// </summary>
    List<string> EncontrarCaminho(string inicio, string alvo)
    {
        Queue<string> fila = new Queue<string>();
        Dictionary<string, string> veioDe = new Dictionary<string, string>();
        HashSet<string> visitados = new HashSet<string>();

        fila.Enqueue(inicio);
        visitados.Add(inicio);

        while (fila.Count > 0)
        {
            string atual = fila.Dequeue();

            if (atual == alvo)
            {
                // reconstrói o caminho
                List<string> caminho = new List<string>();
                string c = alvo;
                while (c != null)
                {
                    caminho.Add(c);
                    veioDe.TryGetValue(c, out c);
                }
                caminho.Reverse();
                return caminho;
            }

            if (!wordBank.grafo.ContainsKey(atual))
                continue;

            foreach (string vizinho in wordBank.grafo[atual])
            {
                if (visitados.Contains(vizinho)) continue;

                fila.Enqueue(vizinho);
                visitados.Add(vizinho);
                veioDe[vizinho] = atual;
            }
        }

        Debug.LogWarning($"Nenhum caminho encontrado entre '{inicio}' e '{alvo}'!");
        return new List<string>() { inicio };
    }
}
