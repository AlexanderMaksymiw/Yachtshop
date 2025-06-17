< !DOCTYPE html >
< html lang = "en" >
< head >
  < meta charset = "UTF-8" />
  < title > CSV Cleaner Upload</title>
  <style>
    body {
      font-family: sans - serif;
padding: 20px;
    }
    .drop - zone {
border: 2px dashed #aaa;
      padding: 30px;
    text - align: center;
color: #555;
      cursor: pointer;
    margin - bottom: 10px;
}
    .drop - zone.dragover {
    background - color: #f0f8ff;
      border - color: #3399ff;
    }
# progress {
width: 100 %;
height: 20px;
background: #f0f0f0;
      margin - top: 10px;
border - radius: 5px;
overflow: hidden;
display: none;
    }
    #progressBar {
      height: 100 %;
background: #4caf50;
      width: 0 %;
    }
    table {
      margin-top: 20px;
border - collapse: collapse;
width: 100 %;
    }
    th, td {
      border: 1px solid #ddd;
      padding: 8px;
    }
    th {
      background-color: #f2f2f2;
    }
  </ style >
</ head >
< body >
  < h2 > Upload CSV to Clean</h2>

  <div class= "drop-zone" id = "dropZone" >
    Drag & drop CSV here, or click to choose
    <input type="file" id="csvFile" accept=".csv" style="display: none;" />
  </div>

  <button onclick="uploadCsv()">Upload & Clean</button>

  <div id="progress">
    <div id="progressBar"></div>
  </div>

  <h3 id="previewTitle" style="display:none;">Preview of Cleaned CSV:</ h3 >
  < div id = "previewContainer" ></ div >

  < script >
    const dropZone = document.getElementById('dropZone');
const fileInput = document.getElementById('csvFile');
const progressBar = document.getElementById('progressBar');
const progressContainer = document.getElementById('progress');
const previewTitle = document.getElementById('previewTitle');
const previewContainer = document.getElementById('previewContainer');

dropZone.addEventListener('click', () => fileInput.click());

dropZone.addEventListener('dragover', (e) => {
    e.preventDefault();
    dropZone.classList.add('dragover');
});

dropZone.addEventListener('dragleave', () => {
    dropZone.classList.remove('dragover');
});

dropZone.addEventListener('drop', (e) => {
    e.preventDefault();
    dropZone.classList.remove('dragover');
    if (e.dataTransfer.files.length)
    {
        fileInput.files = e.dataTransfer.files;
    }
});

async function uploadCsv()
{
    if (!fileInput.files.length)
    {
        alert('Please select or drop a CSV file.');
        return;
    }

    const file = fileInput.files[0];
    const formData = new FormData();
    formData.append('file', file);

    progressContainer.style.display = 'block';
    progressBar.style.width = '0%';
    previewContainer.innerHTML = '';
    previewTitle.style.display = 'none';

    try
    {
        const response = await fetch('/api/csv/clean', {
        method: 'POST',
          body: formData
        });

        if (!response.ok)
        {
            const err = await response.text();
            alert('Error: ' + err);
            progressContainer.style.display = 'none';
            return;
        }

        progressBar.style.width = '100%';

        const blob = await response.blob();

        // Show preview
        const text = await blob.text();
        const rows = text.split('\n').filter(r => r.trim().length).slice(0, 5);
        const table = document.createElement('table');

        rows.forEach((row, idx) => {
            const tr = document.createElement('tr');
            const cells = row.split(',');
            cells.forEach(cell => {
                const td = document.createElement(idx === 0 ? 'th' : 'td');
                td.textContent = cell;
                tr.appendChild(td);
            });
            table.appendChild(tr);
        });

        previewContainer.appendChild(table);
        previewTitle.style.display = 'block';

        // Allow download
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `cleaned_${ file.name}`;
        document.body.appendChild(a);
        a.click();
        a.remove();
        window.URL.revokeObjectURL(url);

        progressContainer.style.display = 'none';
        progressBar.style.width = '0%';

    }
    catch (error)
    {
        alert('Upload failed: ' + error);
        progressContainer.style.display = 'none';
    }
}
  </ script >
</ body >
</ html >
