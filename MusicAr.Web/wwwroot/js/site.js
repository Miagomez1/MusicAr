document.addEventListener("DOMContentLoaded", function () {
    const audioElement = document.getElementById("audio");

    // Botones y elementos del Mini Player
    const playPauseBtn = document.getElementById("btn-play-pause");
    const playerTitle = document.getElementById("player-title");
    const playerArtist = document.getElementById("player-artist");
    const playerCover = document.getElementById("player-cover");
    const progressBar = document.getElementById("progress-bar");
    const progressContainer = document.getElementById("progress-bar-container");

    // Botones y elementos del Player Expandido
    const expandedPlayPauseBtn = document.getElementById("expanded-btn-play-pause");
    const expandedTitle = document.getElementById("expanded-title");
    const expandedArtist = document.getElementById("expanded-artist");
    const expandedCover = document.getElementById("expanded-cover");
    const expandedProgressBar = document.getElementById("expanded-progress-bar");
    const expandedProgressContainer = document.getElementById("expanded-progress-container");
    const currentTimeEl = document.getElementById("current-time");
    const totalDurationEl = document.getElementById("total-duration");

    if (!audioElement) return;

    // Función auxiliar para formatear segundos a MM:SS
    function formatTime(seconds) {
        if (isNaN(seconds)) return "0:00";
        const minutes = Math.floor(seconds / 60);
        const secs = Math.floor(seconds % 60);
        return `${minutes}:${secs < 10 ? '0' : ''}${secs}`;
    }

    // 1. Reproducir al hacer clic en cualquier fila de canción
    document.querySelectorAll(".song-row").forEach(row => {
        row.addEventListener("click", function () {
            const audioUrl = this.getAttribute("data-audio-url");
            const title = this.getAttribute("data-title");
            const artist = this.getAttribute("data-artist");
            const cover = this.getAttribute("data-cover");

            if (!audioUrl) return;

            // Actualizamos textos e imágenes en AMBOS reproductores
            if (playerTitle) playerTitle.textContent = title;
            if (expandedTitle) expandedTitle.textContent = title;

            if (playerArtist) playerArtist.textContent = artist;
            if (expandedArtist) expandedArtist.textContent = artist;

            if (cover) {
                if (playerCover) playerCover.style.backgroundImage = `url('${cover}')`;
                if (expandedCover) expandedCover.style.backgroundImage = `url('${cover}')`;
            }

            // Reproducir audio
            audioElement.src = audioUrl;
            audioElement.play().catch(err => console.error("Error al reproducir:", err));
        });
    });

    // 2. Control unificado de Play / Pausa
    function togglePlayPause() {
        if (!audioElement.src) return;
        if (audioElement.paused) {
            audioElement.play();
        } else {
            audioElement.pause();
        }
    }

    if (playPauseBtn) playPauseBtn.addEventListener("click", togglePlayPause);
    if (expandedPlayPauseBtn) expandedPlayPauseBtn.addEventListener("click", togglePlayPause);

    // 3. Actualizar barras de progreso y tiempos
    audioElement.addEventListener("timeupdate", function () {
        if (audioElement.duration) {
            const percent = (audioElement.currentTime / audioElement.duration) * 100;

            if (progressBar) progressBar.style.width = `${percent}%`;
            if (expandedProgressBar) expandedProgressBar.style.width = `${percent}%`;

            if (currentTimeEl) currentTimeEl.textContent = formatTime(audioElement.currentTime);
            if (totalDurationEl) totalDurationEl.textContent = formatTime(audioElement.duration);
        }
    });

    // 4. Saltar en la canción haciendo clic en las barras de progreso
    function seekAudio(e, container) {
        if (!audioElement.duration) return;
        const rect = container.getBoundingClientRect();
        const clickX = e.clientX - rect.left;
        audioElement.currentTime = (clickX / rect.width) * audioElement.duration;
    }

    if (progressContainer) {
        progressContainer.addEventListener("click", (e) => seekAudio(e, progressContainer));
    }
    if (expandedProgressContainer) {
        expandedProgressContainer.addEventListener("click", (e) => seekAudio(e, expandedProgressContainer));
    }
});