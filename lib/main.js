function Main()
{
    this.install = function()
    {
        let header = `
        <div id="header">
            <div id="photo"></div>
            <div id="title"></div>
        </div>`;

        let content = `
            <div id="content">
                <div id="sidebar"></div>
                <div id="presentation"></div>
            </div>`;
        
        let footer = `
        <div id="footer">
            <div id="footerwrap">
            <div class="credit">Name © ${new Date().getFullYear()}</div>
            </div>
        </div>`;

        document.body.innerHTML += `<div id="view">` + header + content + footer + `</div>`;
        Build(game);
    }
}

function Build(src)
{
    let facts = `
    <div id="column">
        <h2>Website</h2>
        <p><a href="http://${src.website}">${src.website}</a></p>
        <h2>Release Date</h2>
        <p>${src.release}</p>
        <h2>Price</h2>
        <p>${src.price}</p>
        <h2>Platforms</h2>
        <p>${src.platforms.map((item, i) => ` ${src.platforms[i]}`)}</p>

        <h2>Press Assets</h2>
        <p><a href="${src.presskit}">${src.presskit}</a></p>
    </div>`;

    let main = `
        <div id="section">
            <p>${src.logline}</p>
            ${src.description.map((item, i) => `<p>${src.description[i]}</p>`).join('')}
        </div>
        <div id="section">
            <h2>Download</h2>
            <iframe frameborder="0" src="https://itch.io/embed/${src.itch}?bg_color=262626&amp;fg_color=e4e4e3&amp;link_color=e4e4e3&amp;border_color=262626&amp" width="600" height="167"></iframe>
        </div>`;

    let trailers = `
        <div id="section">
            <h2>Trailers</h2>
            <iframe width="100%" height="500" src="https://www.youtube.com/embed/${src.trailer}?rel=0" style="max-width:900px" frameborder="0" allow="autoplay; encrypted-media" allowfullscreen></iframe>
        </div>`;

    let screenshots = `
        <div id="section">
            <h2>Screenshots</h2>
            ${src.gallery.map((item, i) => `<img src="content/images/${src.gallery[i]}">`).join('')}
        </div>`;

    let team = `
        <div id="column">
            <h2>Team</h2>
            ${src.team.map((item, i) => `
                <div class="team">
                    <p><b>${src.team[i].name}</b><br>
                    <a href="http://${src.team[i].social}">${src.team[i].social}</a><br>
                    <a href="mailto:${src.team[i].contact}">${src.team[i].contact}</a></p>
                </div>
            `).join('')}</p>
        </div>`;

    document.title = src.title;
    document.getElementById("photo").style.backgroundImage = "url('content/images/" + src.header + "')";
    document.getElementById("title").innerHTML = src.title.toUpperCase();
    document.getElementById("sidebar").innerHTML += facts + team;
    document.getElementById("presentation").innerHTML += main + trailers + screenshots;
}