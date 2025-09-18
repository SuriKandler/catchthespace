declare const confetti: (options: {
    particleCount: number;
    angle: number;
    spread: number;
    origin: { x: number };
    colors: string[];
}) => void;

$(document).ready(function () {

    let btnStart = $("#btnStart");
    let msg = $("#msg");
    let lines = $(".linehorizontal, .linevertical");
    let spaces = $(".space");
    let progressLabels = $(".progress-label");

    let currentTurn: "girl" | "boy" = "girl";
    let gameOver = false;

    interface Box {
        lines: string[];
        space: string;
    }

    const boxes: Box[] = [
        { lines: ["l1", "l4", "l5", "l8"], space: "s1" },
        { lines: ["l2", "l5", "l6", "l9"], space: "s2" },
        { lines: ["l3", "l6", "l7", "l10"], space: "s3" },
        { lines: ["l8", "l11", "l12", "l15"], space: "s4" },
        { lines: ["l9", "l12", "l13", "l16"], space: "s5" },
        { lines: ["l10", "l13", "l14", "l17"], space: "s6" },
        { lines: ["l15", "l18", "l19", "l22"], space: "s7" },
        { lines: ["l16", "l19", "l20", "l23"], space: "s8" },
        { lines: ["l17", "l20", "l21", "l24"], space: "s9" }
    ];
    //btnStart.click(startGame);
    btnStart.on("click", startGame);

    function showCurrentTurn() {
        msg.attr("class", "turn-label mb-3 " + currentTurn)
            .text(currentTurn === "girl" ? "Girl's Turn" : "Boy's Turn");
    }

    function switchTurn() {
        currentTurn = currentTurn === "girl" ? "boy" : "girl";
        showCurrentTurn();
    }

    function updateProgressBar() {
        progressLabels.removeClass("girl boy");

        const girlScore = $(".space.girl").length;
        const boyScore = $(".space.boy").length;

        for (let i = 0; i < girlScore; i++) {
            $(progressLabels[i]).addClass("girl");
        }

        for (let i = 0; i < boyScore; i++) {
            $(progressLabels[progressLabels.length - 1 - i]).addClass("boy");
        }
    }

    function updateScore() {
        const girlScore = $(".space.girl").length;
        const boyScore = $(".space.boy").length;

        $("#girlScore").text(girlScore);
        $("#boyScore").text(boyScore);

        updateProgressBar();
    }

    function checkForWinner() {
        const girlScore = $(".space.girl").length;
        const boyScore = $(".space.boy").length;

        if (girlScore + boyScore === 9) {
            gameOver = true;
            if (girlScore > boyScore) {
                msg.text("Girl Wins!").attr("class", "turn-label mb-3 girl");
                launchConfetti('mediumpurple');
                $(".player-circle.girl").addClass("winner");
            } else {
                msg.text("Boy Wins!").attr("class", "turn-label mb-3 boy");
                launchConfetti('mediumaquamarine');
                $(".player-circle.boy").addClass("winner");
            }
        }
    }

    function checkForCompletedBoxes() {
        let boxCompleted = false;

        boxes.forEach(box => {
            const linesCompleted = box.lines.every(id => {
                const line = $("#" + id);
                return line.hasClass("girl-line") || line.hasClass("boy-line") || line.hasClass("completed-line");
            });

            const space = $("." + box.space);

            if (linesCompleted && !space.hasClass("girl") && !space.hasClass("boy")) {
                space.addClass(currentTurn);
                box.lines.forEach(id => {
                    $("#" + id).addClass("completed-line");
                });
                boxCompleted = true;
            }
        });

        updateScore();

        if (!boxCompleted) {
            switchTurn();
        }

        checkForWinner();
    }

    function handleLineClick(e: JQuery.ClickEvent) {
        if (gameOver) return;

        const line = $(e.target as HTMLElement);
        if (line.hasClass("girl-line") || line.hasClass("boy-line") || line.hasClass("completed-line")) {
            return;
        }

        if (currentTurn === "girl") {
            line.addClass("girl-line");
        } else {
            line.addClass("boy-line");
        }

        checkForCompletedBoxes();
    }

    function startGame() {
        lines.removeClass("girl-line boy-line completed-line");
        spaces.removeClass("girl boy");
        $(".player-circle").removeClass("winner");

        updateScore();

        currentTurn = "girl";
        showCurrentTurn();
        gameOver = false;
        lines.off("click").on("click", handleLineClick);
    }

    // Confetti launcher
    function launchConfetti(color: string): void {
        if (typeof (window as any).confetti === 'undefined') {
            alert("Confetti! Imagine colorful " + color + " confetti here 🎉");
            return;
        }

        const duration = 2000;
        const end = Date.now() + duration;

        (function frame() {
            confetti({
                particleCount: 5,
                angle: 60,
                spread: 55,
                origin: { x: 0 },
                colors: [color]
            });
            confetti({
                particleCount: 5,
                angle: 120,
                spread: 55,
                origin: { x: 1 },
                colors: [color]
            });

            if (Date.now() < end) {
                requestAnimationFrame(frame);
            }
        })();
    }

});