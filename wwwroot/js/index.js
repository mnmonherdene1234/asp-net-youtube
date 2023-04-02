const logoutButton = document.getElementById("logout-button");
const loginRegisterContainer = document.getElementById(
  "login-register-container"
);
const userEmail = document.getElementById("user-email");

logoutButton.addEventListener("click", () => {
  const logoutXhr = new XMLHttpRequest();

  logoutXhr.onload = function () {
    window.location.replace("/Login");
  };

  logoutXhr.open("GET", "/Index?handler=LogOut");
  logoutXhr.send();
});

const xhr = new XMLHttpRequest();

xhr.onload = function () {
  const response = JSON.parse(this.response);

  if (response?.message === "USER_NOT_FOUND") {
    logoutButton.style.display = "none";
  } else {
    userEmail.textContent = response?.email;
    loginRegisterContainer.style.display = "none";
  }
};

xhr.open("GET", "/Index?handler=User");
xhr.send();
