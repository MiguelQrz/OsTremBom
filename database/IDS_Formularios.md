# 📊 IDs para Integração com Banco de Dados - TREM BOM

## 🔐 AUTENTICAÇÃO

### Login
| ID | Tabela | Coluna | Tipo |
|---|---|---|---|
| `loginEmail` | `usuarios` | `email` | VARCHAR(255) |
| `loginSenha` | `usuarios` | `senha_hash` | VARCHAR(255) |
| `lembrarLogin` | `sessoes` | `token_lembrar` | VARCHAR(255) |

### Registro
| ID | Tabela | Coluna | Tipo |
|---|---|---|---|
| `registroNome` | `usuarios` | `nome_completo` | VARCHAR(255) |
| `registroEmail` | `usuarios` | `email` | VARCHAR(255) |
| `registroTelefone` | `usuarios` | `telefone` | VARCHAR(20) |
| `registroSenha` | `usuarios` | `senha_hash` | VARCHAR(255) |
| `registroFoto` | `usuarios` | `foto_perfil_url` | VARCHAR(500) |
| `preferencias` | `usuarios_preferencias` | `preferencia` | VARCHAR(50) |
| `termosRegistro` | `usuarios` | `termos_aceitos_em` | DATETIME |

## 👤 USUÁRIO (SESSÃO)
| ID | Tabela | Coluna |
|---|---|---|
| `usuarioId` | `usuarios` | `id` |
| `usuarioNome` | `usuarios` | `nome_completo` |
| `usuarioEmail` | `usuarios` | `email` |
| `usuarioTelefone` | `usuarios` | `telefone` |
| `usuarioAvatar` | `usuarios` | `foto_perfil_url` |
| `usuarioPreferencias` | `usuarios_preferencias` | `preferencia` |
| `tokenSessao` | `sessoes` | `token` |

## 🏷️ PREFERÊNCIAS
| Valor | Descrição |
|---|---|
| `barzinho` | 🍻 Barzinho |
| `bar` | 🥃 Bar |
| `samba` | 🥁 Samba |
| `rock` | 🎸 Rock |
| `jazz` | 🎷 Jazz |
| `cafe` | ☕ Café |
| `adega` | 🍷 Adega |
| `teatro` | 🎭 Teatro |
| `cinema` | 🎬 Cinema |

## 🔘 BOTÕES DE NAVEGAÇÃO
| ID | Função |
|---|---|
| `btnLoginHeader` | Botão "Entrar" |
| `btnPerfilHeader` | Botão "Perfil" (logado) |
| `linkPerfil` | Link "Perfil" na navbar |
| `sidebarLinkPerfil` | Link "Perfil" na sidebar |
| `btnLogoutSidebar` | Botão "Sair" |
| `btnExplorarLocais` | Botão "Explorar locais" |
| `btnExplorarCostumes` | Botão "Explorar costumes" |

## 📦 SQL - CRIAÇÃO DAS TABELAS

```sql
CREATE TABLE usuarios (
    id INT PRIMARY KEY IDENTITY(1,1),
    nome_completo NVARCHAR(255) NOT NULL,
    email NVARCHAR(255) UNIQUE NOT NULL,
    telefone NVARCHAR(20),
    senha_hash NVARCHAR(255) NOT NULL,
    foto_perfil_url NVARCHAR(500),
    data_cadastro DATETIME DEFAULT GETDATE(),
    termos_aceitos_em DATETIME,
    ultimo_login DATETIME,
    INDEX idx_email (email)
);

CREATE TABLE usuarios_preferencias (
    id INT PRIMARY KEY IDENTITY(1,1),
    usuario_id INT NOT NULL,
    preferencia NVARCHAR(50) NOT NULL,
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id) ON DELETE CASCADE,
    UNIQUE (usuario_id, preferencia)
);

CREATE TABLE sessoes (
    id INT PRIMARY KEY IDENTITY(1,1),
    usuario_id INT NOT NULL,
    token NVARCHAR(500) NOT NULL,
    token_lembrar NVARCHAR(255),
    data_criacao DATETIME DEFAULT GETDATE(),
    data_expiracao DATETIME,
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id) ON DELETE CASCADE,
    INDEX idx_token (token)
);