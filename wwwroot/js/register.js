const form = document.getElementById("form");

form.addEventListener("submit", (event) => {
  event.preventDefault();

  const formData = new FormData(form);

  console.log(formData);

  const xhr = new XMLHttpRequest();

  xhr.onload = function () {
    const response = JSON.parse(this.response);

    if (response?.message === "error") {
      Swal.fire({
        icon: "error",
        title: "Алдаа",
        text: "Сайн нууц үг оруулна уу!",
      });
    } else {
      window.location.replace("/");
    }
  };

  xhr.open("POST", "/Register");
  xhr.send(formData);
});
