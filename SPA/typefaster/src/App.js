import React, { Component } from 'react';
import { HubConnectionBuilder } from '@aspnet/signalr';

import Chat from './newComponents/Chat';
import Game from './newComponents/Game';

// import logo from './logo.svg';
import './App.css';

class App extends Component {

  constructor() {
    super();

    this.state = {
      hubConnection: null,
      gameInProgress : false,
    }
}
componentDidMount = () => {
  const hubConnection = new HubConnectionBuilder()
                      .withUrl('http://localhost:5000/chatHub')
                      .build();

    this.setState({ hubConnection }, () => {
      this.state.hubConnection
      .start()
      .then(() => {
        console.log('Connection started!');
      })
      .catch(err => console.log('Error while establishing connection :(', err));
    });

    console.log(this.state.hubConnection)
}


startGame(id) {
  console.log("ids", id);
  this.setState( {
    gameInProgress : true, 
    gameId : id
  });
}

updateBoard(id) {
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
        <Chat hubConnection={this.state.hubConnection.bind(this) }/>
       </div>
       </div>
      </div>
    );
  }
}

export default App;