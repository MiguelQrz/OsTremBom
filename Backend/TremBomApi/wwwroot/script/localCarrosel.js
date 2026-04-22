async function carregarLocaisCarrossel() {
    const container = document.getElementById('carrossel-locais');

    try {
        // 1. Faz a chamada para a sua API .NET
        const response = await fetch('http://localhost:5207/api/locais'); // Ajuste a porta se necessário
        
        if (!response.ok) throw new Error("Erro ao buscar dados da API");

        const locais = await response.json();

        // 2. Limpa o container (caso tenha algo escrito lá)
        container.innerHTML = '';

        // 3. Loop para criar cada item do carrossel
        locais.forEach(local => {
            // Criamos o elemento do card
            const localCard = document.createElement('div');
            localCard.className = 'card'; // Usando a classe que você já tem no CSS

            localCard.innerHTML = `
                <div class="card-image">
                    <img src="${local.imagemUrl || 'img/default-bh.jpg'}" alt="${local.nome}">
                </div>
                <div class="card-content">
                    <span class="categoria-badge">${local.categoria}</span>
                    <h3>${local.nome}</h3>
                    <p>${local.descricao}</p>
                    <div class="card-footer">
                        <span>❤️ ${local.totalLikes || 0}</span>
                        <button class="btn-ver-mais" onclick="verDetalhes(${local.id})">Ver mais</button>
                    </div>
                </div>
            `;

            container.appendChild(localCard);
        });

    } catch (error) {
        console.error("Erro:", error);
        container.innerHTML = `<p>Uai, deu um erro ao carregar os trens: ${error.message}</p>`;
    }
}

// Inicia a função assim que a página carregar
document.addEventListener('DOMContentLoaded', carregarLocaisCarrossel);

// Função extra para o botão (exemplo)
function verDetalhes(id) {
    window.location.href = `page/detalhes.html?id=${id}`;
}