// =============================================
// PÁGINA SEMANAL - PUBLICAÇÕES E CARROSSEL
// =============================================

// Dados locais (serão substituídos pela API)
let publicacoes = [];
let locais = [];

// =============================================
// RENDERIZAÇÃO DO CARROSSEL DE LOCAIS
// =============================================
async function carregarLocais() {
    const carrosselContainer = document.getElementById('carrossel-locais');
    if (!carrosselContainer) return;

    try {
        const result = await LocalService.getLocais();
        
        if (result.ok && result.data.sucesso) {
            locais = result.data.dados || [];
            renderizarCarrossel();
        } else {
            carrosselContainer.innerHTML = `<div class="nenhum-local">Nenhum local cadastrado ainda.</div>`;
        }
    } catch (error) {
        console.error('Erro ao carregar locais:', error);
        carrosselContainer.innerHTML = `<div class="nenhum-local">Erro ao carregar locais.</div>`;
    }
}

function renderizarCarrossel() {
    const carrosselContainer = document.getElementById('carrossel-locais');
    if (!carrosselContainer) return;

    if (locais.length === 0) {
        carrosselContainer.innerHTML = `<div class="nenhum-local">Nenhum local cadastrado ainda.</div>`;
        return;
    }

    carrosselContainer.innerHTML = locais.map(local => `
        <div class="Card" id="card-local-${local.id}">
            <div class="card-image">
                <img src="${local.imagemUrl || 'https://via.placeholder.com/300x200'}" alt="${local.nome}" id="local-imagem-${local.id}">
            </div>
            <div class="card-content">
                <div class="card-category" id="local-categoria-${local.id}">${local.categoria || 'Local'}</div>
                <h3 id="local-nome-${local.id}">${local.nome}</h3>
                <p id="local-descricao-${local.id}">${local.descricao || ''}</p>
                <div class="card-stats">
                    <span>❤️ <span id="local-likes-${local.id}">${FormatUtils.formatarNumero(local.totalLikes || 0)}</span></span>
                    <span>💬 <span id="local-comentarios-${local.id}">${FormatUtils.formatarNumero(local.totalComentarios || 0)}</span></span>
                </div>
            </div>
        </div>
    `).join('');
}

// =============================================
// RENDERIZAÇÃO DAS PUBLICAÇÕES
// =============================================
async function carregarPublicacoes() {
    const container = document.getElementById('publicacoes-container');
    if (!container) return;

    try {
        const result = await PublicacaoService.getPublicacoes();
        
        if (result.ok && result.data.sucesso) {
            publicacoes = result.data.dados || [];
            renderizarPublicacoes();
        } else {
            container.innerHTML = `<div class="nenhuma-publicacao" id="nenhumaPublicacaoMsg"><p>📝 Nenhuma publicação ainda</p></div>`;
        }
    } catch (error) {
        console.error('Erro ao carregar publicações:', error);
        container.innerHTML = `<div class="nenhuma-publicacao"><p>Erro ao carregar publicações.</p></div>`;
    }
}

function renderizarPublicacoes() {
    const container = document.getElementById('publicacoes-container');
    if (!container) return;

    if (publicacoes.length === 0) {
        container.innerHTML = `<div class="nenhuma-publicacao" id="nenhumaPublicacaoMsg"><p>📝 Nenhuma publicação ainda</p></div>`;
        return;
    }

    container.innerHTML = publicacoes.map(pub => `
        <div class="post-duas-colunas" id="post-${pub.id}" data-id="${pub.id}">
            <div class="post-coluna-esquerda">
                <div class="post-imagem-coluna" onclick="abrirFotoModal('${pub.fotos?.[0] || 'https://via.placeholder.com/600x400'}')">
                    <img src="${pub.fotos?.[0] || 'https://via.placeholder.com/600x400'}" alt="Foto do post" id="post-imagem-${pub.id}">
                </div>
            </div>
            
            <div class="post-coluna-direita">
                <div class="post-header-coluna">
                    <div class="post-user-coluna">
                        <img class="post-avatar-coluna" src="${pub.usuarioAvatar || 'https://via.placeholder.com/40'}" alt="${pub.usuarioNome}" id="post-avatar-${pub.id}" onclick="irParaPerfil(${pub.usuarioId})">
                        <div class="post-user-info-coluna">
                            <a href="#" onclick="irParaPerfil(${pub.usuarioId}); return false;" class="post-username-coluna" id="post-autor-${pub.id}">${pub.usuarioUsername || '@usuario'}</a>
                            <span class="post-local-coluna" id="post-local-${pub.id}">📍 ${pub.localNome || 'Local'}</span>
                        </div>
                    </div>
                    <button class="post-menu-coluna" id="post-menu-${pub.id}" onclick="abrirMenuPost(${pub.id})">•••</button>
                </div>
                
                <div class="post-actions-coluna">
                    <button class="action-btn-coluna like-btn-coluna ${pub.usuarioCurtiu ? 'liked' : ''}" id="btn-like-${pub.id}" onclick="curtirPublicacao(${pub.id})">
                        <svg width="24" height="24" viewBox="0 0 24 24" fill="${pub.usuarioCurtiu ? 'currentColor' : 'none'}" stroke="currentColor" stroke-width="2">
                            <path d="M20.84 4.61a5.5 5.5 0 0 0-7.78 0L12 5.67l-1.06-1.06a5.5 5.5 0 0 0-7.78 7.78l1.06 1.06L12 21.23l7.78-7.78 1.06-1.06a5.5 5.5 0 0 0 0-7.78z"/>
                        </svg>
                        <span>Curtir</span>
                    </button>
                    <button class="action-btn-coluna" id="btn-comentar-${pub.id}" onclick="abrirComentarios(${pub.id})">
                        <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                            <path d="M21 11.5a8.38 8.38 0 0 1-.9 3.8 8.5 8.5 0 0 1-7.6 4.7 8.38 8.38 0 0 1-3.8-.9L3 21l1.9-5.7a8.38 8.38 0 0 1-.9-3.8 8.5 8.5 0 0 1 4.7-7.6 8.38 8.38 0 0 1 3.8-.9h.5a8.48 8.48 0 0 1 8 8v.5z"/>
                        </svg>
                        <span>Comentar</span>
                    </button>
                </div>
                
                <div class="post-likes-coluna">
                    <span class="likes-count-coluna" id="post-likes-${pub.id}">${FormatUtils.formatarNumero(pub.totalLikes)} curtidas</span>
                </div>
                
                <div class="post-caption-coluna">
                    <a href="#" onclick="irParaPerfil(${pub.usuarioId}); return false;" class="caption-username-coluna">${pub.usuarioUsername || '@usuario'}</a>
                    <span class="caption-text-coluna" id="post-conteudo-${pub.id}">${pub.feedback || ''}</span>
                </div>
                
                <div class="post-comments-coluna" id="post-comentarios-container-${pub.id}">
                    ${pub.comentarios && pub.comentarios.length > 0 ? `
                        <button class="view-comments-coluna" id="ver-comentarios-${pub.id}" onclick="verTodosComentarios(${pub.id})">
                            Ver todos os ${pub.totalComentarios} comentários
                        </button>
                        ${pub.comentarios.slice(0, 2).map(com => `
                            <div class="comment-item-coluna" id="comentario-${pub.id}-${com.id}">
                                <a href="#" onclick="irParaPerfil(${com.usuarioId}); return false;" class="comment-username-coluna">${com.usuarioUsername}</a>
                                <span class="comment-text-coluna">${com.texto}</span>
                                <span class="comment-time-coluna">${com.dataFormatada}</span>
                            </div>
                        `).join('')}
                    ` : `
                        <div class="sem-comentarios-coluna" id="sem-comentarios-${pub.id}">Nenhum comentário ainda. Seja o primeiro!</div>
                    `}
                </div>
                
                <div class="post-footer-coluna">
                    <div class="post-time-coluna" id="post-data-${pub.id}">${pub.dataFormatada || FormatUtils.formatarData(pub.dataCriacao)}</div>
                    <div class="post-rating-coluna" id="post-rating-${pub.id}">
                        ${FormatUtils.gerarEstrelas(pub.rating)}
                        <span class="rating-text-coluna">Avaliação</span>
                    </div>
                </div>
            </div>
        </div>
    `).join('');
}

// =============================================
// AÇÕES DE PUBLICAÇÃO (VIA API)
// =============================================

async function curtirPublicacao(id) {
    if (!AuthService.isAuthenticated()) {
        mostrarToast('Faça login para curtir publicações', 'erro');
        return;
    }

    const publicacao = publicacoes.find(p => p.id === id);
    const btnLike = document.querySelector(`#btn-like-${id}`);
    
    try {
        let result;
        if (publicacao?.usuarioCurtiu) {
            result = await PublicacaoService.descurtirPublicacao(id);
        } else {
            result = await PublicacaoService.curtirPublicacao(id);
        }
        
        if (result.ok && result.data.sucesso) {
            await carregarPublicacoes();
            mostrarToast(publicacao?.usuarioCurtiu ? 'Like removido' : 'Você curtiu! ❤️', 'sucesso');
        }
    } catch (error) {
        console.error('Erro ao curtir:', error);
        mostrarToast('Erro ao processar like', 'erro');
    }
}

async function adicionarComentario(postId) {
    if (!AuthService.isAuthenticated()) {
        mostrarToast('Faça login para comentar', 'erro');
        return;
    }

    const input = document.getElementById(`novoComentarioInput-${postId}`);
    const texto = input?.value.trim();
    
    if (!texto) return;

    try {
        const result = await PublicacaoService.adicionarComentario(postId, texto);
        
        if (result.ok && result.data.sucesso) {
            await carregarPublicacoes();
            
            const modal = document.getElementById(`modalComentarios-${postId}`);
            if (modal) modal.remove();
            
            mostrarToast('Comentário adicionado! 💬', 'sucesso');
        } else {
            mostrarToast(result.data.mensagem || 'Erro ao comentar', 'erro');
        }
    } catch (error) {
        console.error('Erro ao comentar:', error);
        mostrarToast('Erro ao adicionar comentário', 'erro');
    }
}

async function publicarPost(event) {
    event.preventDefault();
    
    if (!AuthService.isAuthenticated()) {
        mostrarToast('Faça login para publicar', 'erro');
        window.location.href = 'login.html';
        return;
    }

    const localSelect = document.getElementById('publicacaoLocal');
    const localId = localSelect.value;
    const localNome = localSelect.options[localSelect.selectedIndex]?.text || '';
    const feedback = document.getElementById('publicacaoFeedback').value;
    const rating = document.querySelector('input[name="rating"]:checked');
    const fotos = document.getElementById('publicacaoFotos').files;

    if (!localId || !feedback || !rating) {
        mostrarToast('Preencha todos os campos!', 'erro');
        return;
    }

    const formData = new FormData();
    formData.append('LocalId', localId);
    formData.append('LocalNome', localNome);
    formData.append('Feedback', feedback);
    formData.append('Rating', rating.value);

    for (let i = 0; i < fotos.length; i++) {
        formData.append('Fotos', fotos[i]);
    }

    const submitBtn = document.querySelector('#formPublicacao button[type="submit"]');
    const originalText = submitBtn.textContent;
    submitBtn.textContent = 'Publicando...';
    submitBtn.disabled = true;

    try {
        const result = await PublicacaoService.criarPublicacao(formData);
        
        if (result.ok && result.data.sucesso) {
            await carregarPublicacoes();
            toggleModalPublicacao();
            document.getElementById('formPublicacao').reset();
            mostrarToast('Publicação realizada! 🎉', 'sucesso');
        } else {
            mostrarToast(result.data.mensagem || 'Erro ao publicar', 'erro');
        }
    } catch (error) {
        console.error('Erro ao publicar:', error);
        mostrarToast('Erro ao criar publicação', 'erro');
    } finally {
        submitBtn.textContent = originalText;
        submitBtn.disabled = false;
    }
}

// =============================================
// MODAIS E UTILITÁRIOS
// =============================================

function abrirComentarios(postId) {
    const post = publicacoes.find(p => p.id === postId);
    if (!post) return;

    const modalComentarios = document.createElement('div');
    modalComentarios.className = 'comentarios-modal-overlay';
    modalComentarios.id = `modalComentarios-${postId}`;
    modalComentarios.innerHTML = `
        <div class="comentarios-modal">
            <div class="comentarios-modal-header">
                <div class="comentarios-header-info">
                    <img class="comentarios-modal-avatar" src="${post.usuarioAvatar || 'https://via.placeholder.com/40'}" alt="${post.usuarioNome}">
                    <div>
                        <h3>${post.usuarioUsername}</h3>
                        <span>📍 ${post.localNome}</span>
                    </div>
                </div>
                <button class="fechar-comentarios" onclick="this.closest('.comentarios-modal-overlay').remove()">✕</button>
            </div>
            <div class="comentarios-list" id="comentariosList-${postId}">
                ${post.comentarios?.map(com => `
                    <div class="comentario-item">
                        <img class="comentario-avatar" src="${com.usuarioAvatar || 'https://via.placeholder.com/40'}" alt="${com.usuarioNome}">
                        <div class="comentario-content">
                            <div class="comentario-header">
                                <a href="#" onclick="irParaPerfil(${com.usuarioId}); return false;" class="comentario-usuario">${com.usuarioUsername}</a>
                                <span class="comentario-data">${com.dataFormatada}</span>
                            </div>
                            <span class="comentario-texto">${com.texto}</span>
                        </div>
                    </div>
                `).join('') || '<div class="sem-comentarios-modal">Nenhum comentário ainda.</div>'}
            </div>
            <div class="comentarios-modal-footer">
                <input type="text" id="novoComentarioInput-${postId}" placeholder="Adicione um comentário..." class="comentario-input">
                <button class="btn-enviar-comentario" onclick="adicionarComentario(${postId})">Publicar</button>
            </div>
        </div>
    `;
    document.body.appendChild(modalComentarios);
}

function verTodosComentarios(postId) {
    abrirComentarios(postId);
}

function abrirFotoModal(imgSrc) {
    const modal = document.getElementById('fotoModal');
    const modalImg = document.getElementById('fotoModalImg');
    if (modal && modalImg) {
        modalImg.src = imgSrc;
        modal.classList.add('active');
    }
}

function fecharFotoModal() {
    const modal = document.getElementById('fotoModal');
    if (modal) modal.classList.remove('active');
}

function toggleModalPublicacao() {
    if (!AuthService.isAuthenticated()) {
        mostrarToast('Faça login para publicar', 'erro');
        window.location.href = '../page/login.html';
        return;
    }
    
    const modal = document.getElementById('modalPublicacao');
    if (modal) {
        modal.classList.toggle('active');
        if (!modal.classList.contains('active')) {
            document.getElementById('formPublicacao')?.reset();
        }
    }
}

function irParaPerfil(userId) {
    if (userId) {
        window.location.href = `../page/perfil.html?id=${userId}`;
    }
}

function abrirMenuPost(id) {
    const post = publicacoes.find(p => p.id === id);
    console.log('Menu do post:', id, post?.usuarioId);
    mostrarToast('Opções do post em desenvolvimento');
}

function mostrarToast(mensagem, tipo = 'info') {
    const toast = document.createElement('div');
    toast.textContent = mensagem;
    toast.id = 'toastMensagem';
    toast.style.cssText = `
        position: fixed;
        bottom: 30px;
        left: 50%;
        transform: translateX(-50%);
        padding: 14px 28px;
        border-radius: 50px;
        font-size: 14px;
        font-weight: 500;
        z-index: 3000;
        animation: slideUp 0.3s ease;
        box-shadow: 0 4px 20px rgba(0,0,0,0.15);
        ${tipo === 'sucesso' ? 'background: #069E6E; color: white;' : 
          tipo === 'erro' ? 'background: #e53e3e; color: white;' : 
          'background: #2D2E47; color: white;'}
    `;
    document.body.appendChild(toast);
    setTimeout(() => {
        toast.style.animation = 'slideDown 0.3s ease';
        setTimeout(() => toast.remove(), 300);
    }, 3000);
}

// =============================================
// INICIALIZAÇÃO
// =============================================
document.addEventListener('DOMContentLoaded', async () => {
    await carregarLocais();
    await carregarPublicacoes();

    const form = document.getElementById('formPublicacao');
    if (form) form.addEventListener('submit', publicarPost);

    const ratingInputs = document.querySelectorAll('input[name="rating"]');
    ratingInputs.forEach(input => {
        input.addEventListener('change', function() {
            document.getElementById('publicacaoRatingSelecionado').value = this.value;
        });
    });
});

// Expor funções globalmente
window.toggleModalPublicacao = toggleModalPublicacao;
window.curtirPublicacao = curtirPublicacao;
window.abrirComentarios = abrirComentarios;
window.adicionarComentario = adicionarComentario;
window.verTodosComentarios = verTodosComentarios;
window.abrirFotoModal = abrirFotoModal;
window.fecharFotoModal = fecharFotoModal;
window.irParaPerfil = irParaPerfil;
window.abrirMenuPost = abrirMenuPost;