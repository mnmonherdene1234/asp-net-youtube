const logoutButton = document.getElementById("logout-button");
const loginRegisterContainer = document.getElementById(
  "login-register-container"
);
const searchInput = document.getElementById("search-input");
const videosContainer = document.getElementById("videos-container");

if (videosContainer) {
  videosContainer.style.display = "flex";
  videosContainer.style.flexWrap = "wrap";
  videosContainer.style.gap = "1rem";
  videosContainer.style.padding = "1rem 0";
}

function getYouTubeId(url) {
  var regExp = /^.*(?:youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=)([^#\&\?]*).*/;
  var match = url.match(regExp);
  if (match && match[1]) {
    return match[1];
  } else {
    return null;
  }
}

const videoCard = (id, title, url, youtubeUrl) => {
  const container = document.createElement("a");
  container.href = `Videos?id=${id}`;
  container.style.width = "200px";
  container.style.border = "solid 1px gray";
  container.style.borderRadius = "10px";
  container.style.overflow = "hidden";
  container.style.textDecoration = "none";

  if (youtubeUrl) {
    const iframe = document.createElement("iframe");
    iframe.src = `https://www.youtube.com/embed/${getYouTubeId(youtubeUrl)}`;
    iframe.width = "200";
    iframe.height = "112";
    iframe.frameBorder = 0;
    container.append(iframe);
  } else {
    const video = document.createElement("video");
    video.src = url;
    video.style.width = "200px";
    container.append(video);
  }

  const pTitle = document.createElement("p");
  pTitle.style.textAlign = "center";
  pTitle.style.margin = "5px";
  pTitle.style.textOverflow = "ellipsis";
  pTitle.style.color = "black";
  pTitle.innerHTML = title;
  container.append(pTitle);

  return container;
};

logoutButton.addEventListener("click", () => {
  const logoutXhr = new XMLHttpRequest();

  logoutXhr.onload = function () {
    window.location.replace("/Login");
  };

  logoutXhr.open("GET", "/Index?handler=LogOut");
  logoutXhr.send();
});

const getVideos = (q) => {
  const xhr = new XMLHttpRequest();

  xhr.onload = function () {
    const response = JSON.parse(this.response);

    videosContainer.innerHTML = "";

    for (const video of response) {
      videosContainer.append(
        videoCard(video?.id, video?.title, video?.url, video?.youtubeUrl)
      );
    }
  };

  xhr.open("GET", `/Index?handler=Videos&q=${q ?? ""}`);
  xhr.send();
};

getVideos();

searchInput.addEventListener("change", async (event) => {
  getVideos(event.target.value?.trim());
});
