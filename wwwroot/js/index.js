const logoutButton = document.getElementById("logout-button");

logoutButton.addEventListener("click", () => {
  const logoutXhr = new XMLHttpRequest();

  logoutXhr.onload = function () {
    const response = JSON.parse(this.response);

    console.log(response);
  };

  logoutXhr.open("GET", "/Index?handler=LogOut");
  logoutXhr.send();
});

const xhr = new XMLHttpRequest();

xhr.onload = function () {
  console.log(JSON.parse(this.response));
};

xhr.open("GET", "/Index?handler=User");
xhr.send();
