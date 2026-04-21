document.getElementById("btnLoginHeader").addEventListener("click", () => {
    const token = localStorage.getItem("token");
    if (token) {
        window.location.href = "/page/perfil.html";
    } else {
        window.location.href = "/page/login.html";
    }
});
