# qb

## Running instructions

- Download the `Build/qb.zip` file, and unzip it
- Navigate to the `qb` directory in the terminal
- Start a local web server in that directory, such as via node.js (faster running):
    - `$ http-server -p 8080`
- Or via Python (slower running):
    - `$ python -m http.server --cgi 8080`
- In your web browser, go to `http://127.0.0.1:8080`. You should see the startup screen and now be able to play.
- Other than the regular play controls, which are presented in the game, in this development version you can force-clear a level by pressing `c`, or force-fail a level (i.e. reset) by pressing `f`
- When finished, go back to the terminal and press `Ctrl-c` to stop the local web server
