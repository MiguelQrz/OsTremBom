// =============================================
// CONTROLE DE AUTENTICAÇÃO - INTERFACE
// =============================================

document.addEventListener('DOMContentLoaded', function() {
    
    // Elementos da interface
    const btnLoginHeader = document.getElementById('btnLoginHeader');
    const btnPerfilHeader = document.getElementById('btnPerfilHeader');
    const linkPerfil = document.getElementById('linkPerfil');
    const sidebarLinkPerfil = document.getElementById('sidebarLinkPerfil');
    const btnLogoutSidebar = document.getElementById('btnLogoutSidebar');
    const btnExplorarLocais = document.getElementById('btnExplorarLocais');
    const btnExplorarCostumes = document.getElementById('btnExplorarCostumes');
    
    // =============================================
    // ATUALIZA INTERFACE BASEADO NA AUTENTICAÇÃO
    // =============================================
    function atualizarInterface() {
        const autenticado = AuthService.isAuthenticated();
        const usuario = AuthService.getUsuario();
        
        if (autenticado) {
            // Usuário logado
            if (btnLoginHeader) btnLoginHeader.style.display = 'none';
            if (btnPerfilHeader) {
                btnPerfilHeader.style.display = 'block';
                btnPerfilHeader.textContent = `👤 ${AuthService.getNomeCurto()}`;
            }
            if (btnLogoutSidebar) btnLogoutSidebar.style.display = 'block';
            if (linkPerfil) linkPerfil.href = 'perfil.html';
            if (sidebarLinkPerfil) sidebarLinkPerfil.href = 'perfil.html';
        } else {
            // Usuário deslogado
            if (btnLoginHeader) btnLoginHeader.style.display = 'block';
            if (btnPerfilHeader) btnPerfilHeader.style.display = 'none';
            if (btnLogoutSidebar) btnLogoutSidebar.style.display = 'none';
            if (linkPerfil) linkPerfil.href = 'login.html';
            if (sidebarLinkPerfil) sidebarLinkPerfil.href = 'login.html';
        }
    }
    
    // =============================================
    // REDIRECIONAMENTOS
    // =============================================
    function redirecionarParaLogin() {
        window.location.href = 'login.html';
    }
    
    function redirecionarParaPerfil() {
        if (AuthService.isAuthenticated()) {
            window.location.href = 'perfil.html';
        } else {
            window.location.href = 'login.html';
        }
    }
    
    async function fazerLogout() {
        if (confirm('Tem certeza que deseja sair?')) {
            await AuthService.logout();
            atualizarInterface();
            
            const paginaAtual = window.location.pathname;
            if (paginaAtual.includes('perfil.html')) {
                window.location.href = 'login.html';
            }
        }
    }
    
    // =============================================
    // EVENT LISTENERS
    // =============================================
    if (btnLoginHeader) {
        btnLoginHeader.addEventListener('click', redirecionarParaLogin);
    }
    
    if (btnPerfilHeader) {
        btnPerfilHeader.addEventListener('click', redirecionarParaPerfil);
    }
    
    if (linkPerfil) {
        linkPerfil.addEventListener('click', function(e) {
            e.preventDefault();
            redirecionarParaPerfil();
        });
    }
    
    if (sidebarLinkPerfil) {
        sidebarLinkPerfil.addEventListener('click', function(e) {
            e.preventDefault();
            redirecionarParaPerfil();
        });
    }
    
    if (btnLogoutSidebar) {
        btnLogoutSidebar.addEventListener('click', fazerLogout);
    }
    
    if (btnExplorarLocais) {
        btnExplorarLocais.addEventListener('click', function() {
            window.location.href = 'locais.html';
        });
    }
    
    if (btnExplorarCostumes) {
        btnExplorarCostumes.addEventListener('click', function() {
            window.location.href = 'semanal.html';
        });
    }
    
    // =============================================
    // INICIALIZAÇÃO
    // =============================================
    atualizarInterface();
    
    window.fazerLogout = fazerLogout;
    window.atualizarInterface = atualizarInterface;
});

// =============================================
// TOGGLE MENU MOBILE
// =============================================
function toggleMenu() {
    const sidebar = document.getElementById('sidebar');
    const overlay = document.getElementById('overlay');
    
    if (sidebar && overlay) {
        sidebar.classList.toggle('active');
        overlay.classList.toggle('active');
        document.body.style.overflow = sidebar.classList.contains('active') ? 'hidden' : '';
    }
}