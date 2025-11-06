using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class WordBank : MonoBehaviour
{
    [Header("Palavra Secreta (defina no Inspector)")]
    public string palavraCorreta = "conhecimento";

    [Header("Lista de Palavras")]
    public List<string> palavras = new List<string>()
    {
        // Objetos e lugares
        "casa", "carro", "livro", "computador", "telefone",
        "janela", "porta", "mesa", "cadeira", "cachorro",
        "gato", "pássaro", "árvore", "flor", "rio",
        "montanha", "praia", "sol", "lua", "estrela",
        "nuvem", "chuva", "vento", "fogo", "água",
        "terra", "ar", "cidade", "vilarejo", "floresta",
        "deserto", "oceano", "ilha", "ponte", "torre",
        "castelo", "templo", "igreja", "ruína", "caverna",
        "porto", "estrada", "mercado", "biblioteca", "escola",
        "hospital", "laboratório", "fábrica", "fazenda", "navio",

        // Atividades e ações
        "trabalho", "estudo", "leitura", "escrita", "pintura",
        "corrida", "salto", "luta", "dança", "canto",
        "voo", "mergulho", "cozinha", "construção", "descanso",
        "sonhar", "viajar", "aprender", "ensinar", "explorar",
        "descobrir", "inventar", "criar", "imaginar", "pensar",

        // Emoções e estados
        "amizade", "amor", "família", "felicidade", "tristeza",
        "raiva", "medo", "coragem", "esperança", "paz",
        "guerra", "ódio", "ciúme", "alegria", "solidão",
        "orgulho", "culpa", "arrependimento", "compaixão", "empatia",
        "inveja", "preguiça", "ganância", "bondade", "maldade",

        // Conceitos filosóficos e abstratos
        "liberdade", "justiça", "verdade", "mentira", "sonho",
        "realidade", "fantasia", "imaginação", "criatividade", "inspiração",
        "energia", "força", "poder", "sabedoria", "conhecimento",
        "inteligência", "curiosidade", "exploração", "descoberta", "inovação",
        "progresso", "futuro", "passado", "presente", "vida",
        "morte", "tempo", "espaço", "universo", "galáxia",
        "planeta", "cometa", "asteroide", "dimensão", "vazio",
        "luz", "sombra", "caos", "ordem", "equilíbrio",
        "destino", "azar", "sorte", "karma", "consciência",
        "memória", "sonho", "pesadelo", "silêncio", "eco",

        // Cultura, arte e ciência
        "música", "filme", "teatro", "arte", "ciência",
        "história", "geografia", "matemática", "física", "química",
        "biologia", "literatura", "poesia", "drama", "comédia",
        "aventura", "mistério", "romance", "suspense", "terror",
        "filosofia", "psicologia", "tecnologia", "engenharia", "robótica",
        "programação", "algoritmo", "dados", "sistema", "rede",
        "internet", "software", "hardware", "código", "inteligência artificial",
        "nanotecnologia", "genética", "astronomia", "astrofísica", "biotecnologia",
        "quântica", "simulação", "realidade virtual", "metaverso", "energia nuclear",

        // Sociedade e política
        "governo", "sociedade", "povo", "cultura", "religião",
        "fé", "lei", "crime", "política", "economia",
        "tradição", "costume", "educação", "pobreza", "riqueza",
        "liderança", "revolução", "opressão", "igualdade", "diversidade",
        "protesto", "libertação", "colonização", "império", "reino",

        // Espaço e ficção científica
        "nave", "robô", "android", "satélite", "buraco negro",
        "hiperespaco", "viagem temporal", "multiverso", "dimensão paralela", "singularidade",
        "inteligência cósmica", "civilização", "planeta morto", "colônia", "terraformação",

        // Natureza e elementos
        "rocha", "areia", "neve", "gelo", "relâmpago",
        "trovão", "raiz", "folha", "tronco", "fruto",
        "ferrugem", "cristal", "ouro", "prata", "ferro",
        "diamante", "carbono", "oxigênio", "hidrogênio", "átomo",

        // Tempo e transformação
        "origem", "criação", "evolução", "mutação", "decadência",
        "renascimento", "transformação", "fim", "eternidade", "ciclo",
    };


    [Header("Grafo Gerado Automaticamente")]
    public Dictionary<string, List<string>> grafo = new Dictionary<string, List<string>>();

    void Awake()
    {
        GerarGrafoAutomaticamente();
    }

    void GerarGrafoAutomaticamente()
    {
        grafo.Clear();

        foreach (string palavra in palavras)
        {
            grafo[palavra] = new List<string>();
        }

        // Conecta cada palavra às mais semelhantes
        foreach (string palavra in palavras)
        {
            List<(string outra, int distancia)> distancias = new List<(string, int)>();

            foreach (string outra in palavras)
            {
                if (palavra == outra) continue;
                int d = CalcularDistanciaLevenshtein(palavra, outra);
                distancias.Add((outra, d));
            }

            // Pega as 3 mais próximas (podes ajustar esse número)
            var vizinhos = distancias.OrderBy(x => x.distancia).Take(3).Select(x => x.outra);
            foreach (string v in vizinhos)
            {
                if (!grafo[palavra].Contains(v))
                    grafo[palavra].Add(v);
                if (!grafo[v].Contains(palavra))
                    grafo[v].Add(palavra);
            }
        }

        Debug.Log($"✅ Grafo gerado automaticamente com {grafo.Count} nós.");
    }

    public string GetPalavraAleatoria()
    {
        if (palavras.Count == 0) return string.Empty;
        return palavras[Random.Range(0, palavras.Count)];
    }

    public int CalcularDistancia(string inicio, string alvo)
    {
        if (inicio == alvo) return 0;
        if (!grafo.ContainsKey(inicio) || !grafo.ContainsKey(alvo)) return -1;

        var fila = new Queue<string>();
        var dist = new Dictionary<string, int>();

        fila.Enqueue(inicio);
        dist[inicio] = 0;

        while (fila.Count > 0)
        {
            var atual = fila.Dequeue();
            int d = dist[atual];

            foreach (var viz in grafo[atual])
            {
                if (dist.ContainsKey(viz)) continue;
                dist[viz] = d + 1;
                if (viz == alvo) return dist[viz];
                fila.Enqueue(viz);
            }
        }
        return -1;
    }

    int CalcularDistanciaLevenshtein(string a, string b)
    {
        int[,] dp = new int[a.Length + 1, b.Length + 1];

        for (int i = 0; i <= a.Length; i++) dp[i, 0] = i;
        for (int j = 0; j <= b.Length; j++) dp[0, j] = j;

        for (int i = 1; i <= a.Length; i++)
        {
            for (int j = 1; j <= b.Length; j++)
            {
                int custo = (a[i - 1] == b[j - 1]) ? 0 : 1;
                dp[i, j] = Mathf.Min(
                    Mathf.Min(dp[i - 1, j] + 1, dp[i, j - 1] + 1),
                    dp[i - 1, j - 1] + custo
                );
            }
        }

        return dp[a.Length, b.Length];
    }

    public float CalcularProximidade(string palavra)
    {
        int dist = CalcularDistancia(palavra, palavraCorreta);
        if (dist < 0) dist = CalcularDistanciaLevenshtein(palavra, palavraCorreta);
        float valor = Mathf.Clamp(100f - dist * 10f, 0, 100f);
        return valor;
    }
}
