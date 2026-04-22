const input = document.getElementById("foto-input");
const preview = document.getElementById("preview");

input.addEventListener("change", () => {
  const file = input.files[0];

  if (file) {
    const reader = new FileReader();

    reader.onload = function (e) {
      preview.src = e.target.result;
    };
    reader.readAsDataURL(file);
  }
});
const tags = document.querySelectorAll(".tag");

tags.forEach(tag => {
  tag.addEventListener("click", () => {
    tag.classList.toggle("selected");
  });
});