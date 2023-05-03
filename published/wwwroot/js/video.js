const id = location.search.slice(4);
const title = document.getElementById("title");
const video = document.getElementById("video");
const username = document.getElementById("username");
const comments = document.getElementById("comments");
const commentContainer = document.getElementById("comment-container");
const comment = document.getElementById("comment");
const sendButton = document.getElementById("send-button");

commentContainer.style.display = "flex";
commentContainer.style.alignItems = "center";
comment.style.borderRadius = "10rem 0 0 10rem";
comment.style.border = "1px solid gray";
comment.style.padding = "5px 10px";

sendButton.style.display = "grid";
sendButton.style.placeItems = "center";
sendButton.style.backgroundColor = "#5443a0";
sendButton.style.color = "white";
sendButton.style.height = "28px";
sendButton.style.aspectRatio = "1 / 1";
sendButton.style.borderRadius = "0 5px 5px 0";
sendButton.style.cursor = "pointer";

const getVideo = () => {
  const xhr = new XMLHttpRequest();

  xhr.onload = () => {
    const data = JSON.parse(xhr.response);
    title.innerHTML = data?.title;
    video.src = data?.url;
  };

  xhr.open("GET", `/Videos?handler=Video&id=${id}`);
  xhr.send();
};

getVideo();

const commentCard = (username, message) => {
  const container = document.createElement("div");
  const text = document.createElement("p");
  text.innerHTML = `${username}: ${message}`;
  container.append(text);

  return container;
};

const getComments = () => {
  const xhr = new XMLHttpRequest();

  xhr.onload = () => {
    const data = JSON.parse(xhr.response);

    if (Array.isArray(data)) {
      comments.innerHTML = "";
      for (const comment of data) {
        comments.append(commentCard(comment?.user?.userName, comment?.text));
      }
    }
  };

  xhr.open("GET", `/Videos?handler=Comments&id=${id}`);
  xhr.send();
};

getComments();

const getUser = () => {
  const xhr = new XMLHttpRequest();

  xhr.onload = () => {
    const data = JSON.parse(xhr.response);
    console.log(data);
    username.innerHTML = data?.userName;
  };

  xhr.open("GET", `/Videos?handler=User&id=${id}`);
  xhr.send();
};

getUser();

const sendComment = () => {
  if (comment.value) {
    const xhr = new XMLHttpRequest();

    xhr.onload = () => {
      getComments();
    };

    xhr.open("POST", "/Videos?handler=SaveComment");

    const formData = new FormData();
    formData.append("comment", comment.value);
    formData.append("id", id);

    xhr.send(formData);
  }
};

sendButton.addEventListener("click", sendComment);
