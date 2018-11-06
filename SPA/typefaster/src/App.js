import React, { Component } from 'react';

import Chat from './newComponents/Chat';
import Game from './newComponents/Game';

// import logo from './logo.svg';
import './App.css';

class App extends Component {

  constructor() {
    super();

    this.state = {
      gameInProgress : false,
    }
}

startGame(id) {
  console.log("ids", id);
  this.setState( {
    gameInProgress : true, 
    gameId : id
  });
}

  render() {
    let game;

    if(this.state.gameInProgress) {
      game =  <Game />
    }


    return (
      <div className="App">
        {/* <header className="App-header">
          <img src={logo} className="App-logo" alt="logo" />
          <h1 className="App-title">Welcome to React</h1>
        </header> */}
       <div>
         {/* <Game verticals="3" horisontals="3" />
         <Chat /> */}
       <div className="GameContainer">
         {game}
       </div>
       <div className="ChatContainer">
        <Chat startGame={this.startGame.bind(this)}/>
       </div>
       </div>
      </div>
    );
  }
}

export default App;