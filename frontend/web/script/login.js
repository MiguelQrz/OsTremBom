// =============================================
// ANIMAÇÕES E VALIDAÇÕES VISUAIS - LOGIN
// =============================================

document.addEventListener('DOMContentLoaded', function() {
    
    // =============================================
    // ELEMENTOS DO DOM
    // =============================================
    const cardFlipContainer = document.getElementById('cardFlipContainer');
    const btnMostrarRegistro = document.getElementById('btnMostrarRegistro');
    const btnMostrarLogin = document.getElementById('btnMostrarLogin');
    const formLogin = document.getElementById('formLogin');
    const formRegistro = document.getElementById('formRegistro');
    
    // Preview da foto
    const registroFoto = document.getElementById('registroFoto');
    const previewImagem = document.getElementById('previewImagem');
    const fotoPlaceholder = document.querySelector('.foto-placeholder');
    
    // Feedback de senha
    const registroSenha = document.getElementById('registroSenha');
    const senhaFeedback = document.getElementById('senhaFeedback');
    
    // =============================================
    // ANIMAÇÃO: FLIP CARD
    // =============================================
    function flipCard(showFront) {
        if (showFront) {
            cardFlipContainer.classList.remove('flipped');
        } else {
            cardFlipContainer.classList.add('flipped');
        }
    }
    
    if (btnMostrarRegistro) {
        btnMostrarRegistro.addEventListener('click', (e) => {
            e.preventDefault();
            flipCard(false);
        });
    }
    
    if (btnMostrarLogin) {
        btnMostrarLogin.addEventListener('click', (e) => {
            e.preventDefault();
            flipCard(true);
        });
    }
    
    // =============================================
    // PREVIEW DA FOTO
    // =============================================
    if (registroFoto) {
        registroFoto.addEventListener('change', function(e) {
            const file = e.target.files[0];
            
            if (file) {
                if (file.size > 5 * 1024 * 1024) {
                    mostrarNotificacao('A imagem deve ter no máximo 5MB.', 'erro');
                    this.value = '';
                    return;
                }
                
                const formatosPermitidos = ['image/jpeg', 'image/png', 'image/gif', 'image/webp'];
                if (!formatosPermitidos.includes(file.type)) {
                    mostrarNotificacao('Formato inválido. Use JPG, PNG, GIF ou WEBP.', 'erro');
                    this.value = '';
                    return;
                }
                
                const reader = new FileReader();
                reader.onload = (event) => {
                    previewImagem.src = event.target.result;
                    previewImagem.style.display = 'block';
                    if (fotoPlaceholder) fotoPlaceholder.style.display = 'none';
                };
                reader.readAsDataURL(file);
            }
        });
    }
    
    // =============================================
    // FEEDBACK DE SENHA
    // =============================================
    function validarSenhaVisual(senha) {
        const erros = [];
        if (senha.length < 8) erros.push('Mínimo 8 caracteres');
        if (!/[A-Z]/.test(senha)) erros.push('1 letra maiúscula');
        if (!/\d/.test(senha)) erros.push('1 número');
        return { valido: erros.length === 0, erros };
    }
    
    if (registroSenha && senhaFeedback) {
        registroSenha.addEventListener('input', function() {
            const validacao = validarSenhaVisual(this.value);
            
            if (this.value.length > 0) {
                if (validacao.valido) {
                    senhaFeedback.textContent = '✅ Senha forte!';
                    senhaFeedback.className = 'password-feedback valido';
                } else {
                    senhaFeedback.textContent = `❌ Requisitos: ${validacao.erros.join(', ')}`;
                    senhaFeedback.className = 'password-feedback invalido';
                }
            } else {
                senhaFeedback.textContent = '';
            }
        });
    }
    
    // =============================================
    // MÁSCARA DE TELEFONE
    // =============================================
    const registroTelefone = document.getElementById('registroTelefone');
    if (registroTelefone) {
        registroTelefone.addEventListener('input', function(e) {
            let valor = e.target.value.replace(/\D/g, '');
            if (valor.length > 11) valor = valor.slice(0, 11);
            
            if (valor.length > 0) {
                if (valor.length <= 2) {
                    valor = `(${valor}`;
                } else if (valor.length <= 6) {
                    valor = `(${valor.slice(0, 2)}) ${valor.slice(2)}`;
                } else if (valor.length <= 10) {
                    valor = `(${valor.slice(0, 2)}) ${valor.slice(2, 6)}-${valor.slice(6)}`;
                } else {
                    valor = `(${valor.slice(0, 2)}) ${valor.slice(2, 7)}-${valor.slice(7, 11)}`;
                }
            }
            e.target.value = valor;
        });
    }
    
    // =============================================
    // SUBMIT: LOGIN (VIA API)
    // =============================================
    if (formLogin) {
        formLogin.addEventListener('submit', async function(e) {
            e.preventDefault();
            
            const email = document.getElementById('loginEmail').value;
            const senha = document.getElementById('loginSenha').value;
            const lembrar = document.getElementById('lembrarLogin')?.checked || false;
            
            if (!email || !senha) {
                mostrarNotificacao('Preencha todos os campos.', 'erro');
                return;
            }
            
            const submitBtn = this.querySelector('button[type="submit"]');
            const originalText = submitBtn.textContent;
            submitBtn.textContent = 'Entrando...';
            submitBtn.disabled = true;
            
            const result = await AuthService.login(email, senha, lembrar);
            
            if (result.ok && result.data.sucesso) {
                AuthService.salvarSessao(result.data.dados.token, result.data.dados.usuario);
                mostrarNotificacao('✅ Login realizado com sucesso!', 'sucesso');
                
                setTimeout(() => {
                    window.location.href = 'perfil.html';
                }, 1500);
            } else {
                mostrarNotificacao(result.data.mensagem || 'Erro ao fazer login', 'erro');
            }
            
            submitBtn.textContent = originalText;
            submitBtn.disabled = false;
        });
    }
    
    // =============================================
    // SUBMIT: REGISTRO (VIA API)
    // =============================================
    if (formRegistro) {
        formRegistro.addEventListener('submit', async function(e) {
            e.preventDefault();
            
            const submitBtn = this.querySelector('button[type="submit"]');
            const originalText = submitBtn.textContent;
            submitBtn.textContent = 'Criando conta...';
            submitBtn.disabled = true;
            
            const formData = new FormData();
            formData.append('NomeCompleto', document.getElementById('registroNome').value);
            formData.append('Email', document.getElementById('registroEmail').value);
            formData.append('Telefone', document.getElementById('registroTelefone').value);
            formData.append('Senha', document.getElementById('registroSenha').value);
            formData.append('ConfirmarSenha', document.getElementById('registroConfirmarSenha').value);
            formData.append('TermosAceitos', document.getElementById('termosRegistro').checked);
            
            const fotoFile = document.getElementById('registroFoto').files[0];
            if (fotoFile) {
                formData.append('FotoPerfil', fotoFile);
            }
            
            const preferencias = Array.from(document.querySelectorAll('input[name="preferencias"]:checked'))
                .map(cb => cb.value);
            
            if (preferencias.length === 0) {
                mostrarNotificacao('Escolha pelo menos uma preferência.', 'erro');
                submitBtn.textContent = originalText;
                submitBtn.disabled = false;
                return;
            }
            
            preferencias.forEach(pref => formData.append('Preferencias', pref));
            
            const result = await AuthService.registro(formData);
            
            if (result.ok && result.data.sucesso) {
                AuthService.salvarSessao(result.data.dados.token, result.data.dados.usuario);
                mostrarNotificacao('✅ Conta criada com sucesso!', 'sucesso');
                
                submitBtn.textContent = '🎉 Conta criada!';
                
                setTimeout(() => {
                    window.location.href = 'perfil.html';
                }, 1500);
            } else {
                mostrarNotificacao(result.data.mensagem || 'Erro ao criar conta', 'erro');
            }
            
            submitBtn.textContent = originalText;
            submitBtn.disabled = false;
        });
    }
    
    // =============================================
    // SISTEMA DE NOTIFICAÇÕES
    // =============================================
    function mostrarNotificacao(mensagem, tipo = 'info') {
        const toast = document.createElement('div');
        toast.textContent = mensagem;
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
    // VERIFICA SE JÁ ESTÁ LOGADO
    // =============================================
    if (AuthService.isAuthenticated()) {
        // window.location.href = 'perfil.html';
    }
    
    // Animação de entrada
    setTimeout(() => {
        document.querySelector('.SessaoAuth').style.opacity = '1';
    }, 100);
});