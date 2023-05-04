const selectButton = document.getElementById("select-button");
const fileInput = document.createElement("input");
fileInput.type = "file";
fileInput.accept = "video/mp4";
const selectedVideo = document.getElementById("selected-video");
const selectedVideoInfo = document.getElementById("selected-video-info");
selectedVideo.style.display = "none";
const uploadButton = document.getElementById("upload-button");
const titleInput = document.getElementById("title-input");
let fileSizeInMB = 0;

selectButton.addEventListener("click", () => {
  fileInput.click();
});

fileInput.addEventListener("change", (event) => {
  const [file] = event.target.files;
  if (file) {
    const fileSizeInBytes = file.size;
    fileSizeInMB = fileSizeInBytes / (1024 * 1024);

    selectedVideoInfo.innerHTML = `Нэр: ${
      file.name
    } Хэмжээ: ${fileSizeInMB.toPrecision(2)}MB`;
    const videoUrl = URL.createObjectURL(file);
    selectedVideo.src = videoUrl;
    selectedVideo.style.display = "block";
  } else {
    selectedVideo.style.display = "none";
    selectedVideoInfo.innerHTML = "Бичлэг сонгон уу?";
  }
});

uploadButton.addEventListener("click", () => {
  const [file] = fileInput.files;

  if (file) {
    if (titleInput.value) {
      if (fileSizeInMB < 100) {
        const xhr = new XMLHttpRequest();

        xhr.onload = () => {
          location.href = "/";
        };

        xhr.open("POST", `/Upload?handler=AddVideo`);

        const formData = new FormData();

        formData.append("title", titleInput.value);
        formData.append("video", file);

        xhr.send(formData);
      } else {
        Swal.fire({
          title: "Алдаа",
          text: "100MB бага байна",
          icon: "error",
          confirmButtonText: "За",
        });
      }
    } else {
      Swal.fire({
        title: "Алдаа",
        text: "Гарчиг оруулна уу!",
        icon: "error",
        confirmButtonText: "За",
      });
    }
  } else {
    Swal.fire({
      title: "Алдаа",
      text: "Бичлэг сонгон уу!",
      icon: "error",
      confirmButtonText: "За",
    });
  }
});
