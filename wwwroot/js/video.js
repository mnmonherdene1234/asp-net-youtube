const id = location.search.slice(4);
const title = document.getElementById("title");
const video = document.getElementById("video");
const username = document.getElementById("username");
const comments = document.getElementById("comments");
const commentContainer = document.getElementById("comment-container");
const comment = document.getElementById("comment");
const sendButton = document.getElementById("send-button");
const editContainer = document.getElementById("edit-container");
const titleInput = document.getElementById("title-input");
const saveButton = document.getElementById("save-button");
const deleteButton = document.getElementById("delete-button");
const iframe = document.getElementById("iframe");

video.style.maxWidth = "100%";

editContainer.style.display = "none";

titleInput.style.borderRadius = "999px";
titleInput.style.border = "1px solid gray";
titleInput.style.padding = "3px 1rem";
titleInput.style.maxWidth = "150px";

saveButton.style.borderRadius = "999px";
saveButton.style.border = "none";

deleteButton.style.borderRadius = "999px";
deleteButton.style.border = "none";

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

let videoUserId = "";
let userId = "";

function getYouTubeId(url) {
  var regExp = /^.*(?:youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=)([^#\&\?]*).*/;
  var match = url.match(regExp);
  if (match && match[1]) {
    return match[1];
  } else {
    return null;
  }
}

const getVideo = async () => {
  const xhr = new XMLHttpRequest();

  xhr.onload = () => {
    const data = JSON.parse(xhr.response);
    title.innerHTML = data?.title;
    titleInput.value = data?.title;

    if (data?.youtubeUrl) {
      video.style.display = "none";
      iframe.src = `https://www.youtube.com/embed/${getYouTubeId(
        data?.youtubeUrl
      )}`;

      iframe.style.width = "100%";
      iframe.style.aspectRatio = "16 / 9";
    } else {
      iframe.style.display = "none";
      video.src = data?.url;
    }

    videoUserId = data?.userId;

    if (videoUserId === userId) {
      editContainer.style.display = "block";
    }
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

const getComments = async () => {
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

const getAuther = async () => {
  const xhr = new XMLHttpRequest();

  xhr.onload = () => {
    const data = JSON.parse(xhr.response);
    username.innerHTML = data?.userName;
  };

  xhr.open("GET", `/Videos?handler=Author&id=${id}`);
  xhr.send();
};

getAuther();

const getUser = async () => {
  const xhr = new XMLHttpRequest();

  xhr.onload = () => {
    const data = JSON.parse(xhr.response);
    userId = data?.id;

    if (userId === videoUserId) {
      editContainer.style.display = "block";
    }
  };

  xhr.open("GET", `/Videos?handler=User`);
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

deleteButton.addEventListener("click", () => {
  Swal.fire({
    title: "Устгах даа итгэлтэй байна уу?",
    showDenyButton: true,
    showCancelButton: true,
    confirmButtonText: "Устгах",
    denyButtonText: `Үгүй`,
  }).then((result) => {
    if (result.isConfirmed) {
      const xhr = new XMLHttpRequest();

      xhr.onload = () => {
        location.href = "/";
      };

      xhr.open("POST", "/Videos?handler=Delete");

      const formData = new FormData();
      formData.append("id", id);

      xhr.send(formData);
    }
  });
});

saveButton.addEventListener("click", () => {
  const xhr = new XMLHttpRequest();

  xhr.onload = () => {
    const data = JSON.parse(xhr.response);

    if (data?.message === "OK") {
      Swal.fire({
        icon: "success",
        title: "Амжилттай",
        showConfirmButton: false,
        timer: 1500,
      });

      title.innerHTML = titleInput.value;
    } else {
      Swal.fire({
        icon: "error",
        title: "Амжилтгүй",
        showConfirmButton: false,
        timer: 1500,
      });
    }
  };

  xhr.open("POST", "/Videos?handler=ChangeTitle");

  const formData = new FormData();
  formData.append("id", id);
  formData.append("title", titleInput.value);

  xhr.send(formData);
});
