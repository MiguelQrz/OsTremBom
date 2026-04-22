document.getElementById("registro-form").addEventListener("submit", function (e) {
  e.preventDefault();

  const nome = document.getElementById("nome").value.trim();
  const sobrenome = document.getElementById("sobrenome").value.trim();
  const email = document.getElementById("email").value.trim();
  const senha = document.getElementById("senha").value.trim();
  const confirmarSenha = document.getElementById("conf-senha").value.trim();
  const error = document.getElementById("error");

  error.textContent = "";

  // validações básicas
  if (name === "" || sobrenome == '') {
    error.textContent = "Por favor, digite um nome válido.";
    return;
  }

  else if (!email.includes("@")) {
    error.textContent = "E-mail inválido";
    return;
  }

  else if (password.length < 6) {
    error.textContent = "Senha precisa ter pelo menos 6 caracteres";
    return;
  }
  else if (password != confirmarSenha){
    error.textContent = "As senhas não coincidem."
    return;
  }

  // se tudo ok
  alert("Formulário válido!");
});