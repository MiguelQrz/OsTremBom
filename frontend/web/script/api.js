// =============================================
// CONFIGURAÇÃO DA API - TREM BOM
// =============================================

const API_CONFIG = {
    BASE_URL: 'https://localhost:5001/api',
    ENDPOINTS: {
        // Auth
        LOGIN: '/Auth/login',
        REGISTRO: '/Auth/registro',
        LOGOUT: '/Auth/logout',
        VERIFICAR_EMAIL: '/Auth/verificar-email',
        ME: '/Auth/me'
    }
};

// =============================================
// CLIENTE HTTP
// =============================================

class ApiClient {
    constructor(baseUrl) {
        this.baseUrl = baseUrl;
    }

    getToken() {
        return localStorage.getItem('token');
    }

    setToken(token) {
        localStorage.setItem('token', token);
    }

    removeToken() {
        localStorage.removeItem('token');
        localStorage.removeItem('usuario');
        localStorage.removeItem('usuarioAutenticado');
    }

    async request(endpoint, options = {}) {
        const url = `${this.baseUrl}${endpoint}`;
        const token = this.getToken();

        const headers = {
            ...options.headers
        };

        if (token) {
            headers['Authorization'] = `Bearer ${token}`;
        }

        const config = {
            ...options,
            headers
        };

        try {
            const response = await fetch(url, config);
            const data = await response.json();
            
            return {
                ok: response.ok,
                status: response.status,
                data: data
            };
        } catch (error) {
            console.error('Erro na API:', error);
            return {
                ok: false,
                status: 0,
                data: { sucesso: false, mensagem: 'Erro de conexão com o servidor' }
            };
        }
    }

    async post(endpoint, body, isFormData = false) {
        const options = {
            method: 'POST'
        };

        if (isFormData) {
            options.body = body;
        } else {
            options.headers = { 'Content-Type': 'application/json' };
            options.body = JSON.stringify(body);
        }

        return this.request(endpoint, options);
    }

    async get(endpoint) {
        return this.request(endpoint, { method: 'GET' });
    }
}

const api = new ApiClient(API_CONFIG.BASE_URL);

// =============================================
// SERVIÇOS DE AUTENTICAÇÃO
// =============================================

const AuthService = {
    async login(email, senha, lembrar = false) {
        return api.post(API_CONFIG.ENDPOINTS.LOGIN, {
            email: email,
            senha: senha,
            lembrar: lembrar
        });
    },

    async registro(formData) {
        return api.post(API_CONFIG.ENDPOINTS.REGISTRO, formData, true);
    },

    async logout() {
        const result = await api.post(API_CONFIG.ENDPOINTS.LOGOUT, {});
        api.removeToken();
        return result;
    },

    async verificarEmail(email) {
        return api.get(`${API_CONFIG.ENDPOINTS.VERIFICAR_EMAIL}?email=${encodeURIComponent(email)}`);
    },

    async getUsuarioAtual() {
        return api.get(API_CONFIG.ENDPOINTS.ME);
    },

    isAuthenticated() {
        const token = api.getToken();
        const usuarioAutenticado = localStorage.getItem('usuarioAutenticado');
        return !!(token && usuarioAutenticado === 'true');
    },

    salvarSessao(token, usuario) {
        api.setToken(token);
        localStorage.setItem('usuario', JSON.stringify(usuario));
        localStorage.setItem('usuarioAutenticado', 'true');
    },

    getUsuario() {
        const usuarioJson = localStorage.getItem('usuario');
        return usuarioJson ? JSON.parse(usuarioJson) : null;
    },

    getNomeCurto() {
        const usuario = this.getUsuario();
        if (usuario && usuario.nomeCompleto) {
            return usuario.nomeCompleto.split(' ')[0];
        }
        return 'Perfil';
    }
};




// =============================================
// EXPORTAÇÃO GLOBAL
// =============================================
window.api = api;
window.AuthService = AuthService;
window.API_CONFIG = API_CONFIG;