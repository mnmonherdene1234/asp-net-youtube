const id = location.search.slice(4);

const getVideo = () => {
  const xhr = new XMLHttpRequest();

  xhr.onload = () => {
    console.log(JSON.parse(xhr.response));
  };

  xhr.open("GET", `/Videos?handler=Video&id=${id}`);
  xhr.send();
};

getVideo();