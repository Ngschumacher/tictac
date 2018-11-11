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
      user : {},
      board : {},
      game : {},
      gameStatus : {},
      connectedUsers : [],
      challenges : [],
      gameInProgress : false,
    }
}
componentDidMount = () => {
  const hubConnection = new HubConnectionBuilder()
                      .withUrl('http://localhost:5000/chatHub')
                      .build();

    
  let nick = this.getNick();
  console.log(nick);

  // fetch(`http://localhost:5000/api/account/login?username=${nick}`)
  //       .then(result => {
  //           console.log(result);
  //           let json = result.json();
  //           return json;

  //       }).then(data => {
  //           console.log("login", data);
  //           this.setState({ user : data.user });
  //       });


    this.setState({ hubConnection, nick }, () => {
      this.state.hubConnection
        .start()
        .then(() => {
          console.log('Connection started!');
          this.state.hubConnection
            .invoke('signIn', this.state.nick)
            .catch(err => console.log(err));
        })
        .catch(err => console.log('Error while establishing connection :(', err));
      
      
        this.state.hubConnection.on('userInformation', (user) => {
        this.setState({user : user});
      })


      this.state.hubConnection.on('sendToAll', (nick, receivedMessage) => {
        const text = `${nick}: ${receivedMessage}`;
        const messages = this.state.messages.concat([text]);
        this.setState({ messages });
      });

      this.state.hubConnection.on('connections', (connections) => {

        this.setState({connectedUsers : connections  });
        
        console.log(connections);
      });

      this.state.hubConnection.on('challengeRecieved', (message) => {
        var joined = this.state.challenges.concat(message);
        this.setState({ challenges: joined });
        console.log("challenges", this.state.challenges);
      });

      this.state.hubConnection.on('updateBoard', (gameInformation) => {
        console.log("updateBoard",gameInformation);
        this.setState({
          board : gameInformation.board,
          game : gameInformation.game,
          gameStatus : gameInformation.gameStatus,
        });
        console.log("state is", this.state);
      });

      this.state.hubConnection.on('gameStarting', (gameModel) => {
        console.log("game started",gameModel);
        this.setState({
          game : gameModel.game,
          board : gameModel.board,
          gameInProgress : true
        })
      });

      // this.state.hubConnection.on('updateBoard', (gameModel) => {
      //   console.log("updateBoard",gameModel);
      //   this.props.updateBoard(gameModel.gameId);
        
      // });

    });
   
    



    // this.setState({ hubConnection : hubConnection }, () => {
    //   this.state.hubConnection
    //   .start()
    //   .then(() => {
    //     console.log('Connection started!');
    //   })
    //   .catch(err => console.log('Error while establishing connection :(', err));
    // });

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

acceptChallenge(userId, gameId) {
  console.log("accepting game", gameId);
  
  fetch(`http://localhost:5000/api/game/AcceptChallenge?AccepterId=${userId}&gameId=${gameId}`)
  .then(result => {
  })
}


  render() {
    let game;

    if(this.state.gameInProgress) {
      game =  
      <Game 
        user={ this.state.user }
        game={ this.state.game }
        board={ this.state.board }
        gameStatus= { this.state.gameStatus }
      />
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
        <Chat 
          user={this.state.user}
          connectedUsers={this.state.connectedUsers}
          challenges={this.state.challenges}
          acceptChallenge={this.acceptChallenge.bind(this)} 
        />
       </div>
       </div>
      </div>
    );
  }




  getNick() {
    let nick = "John";

    // const nick = window.prompt('Your name:', 'John');
    var isChrome = !!window.chrome && !!window.chrome.webstore;
    var isFirefox = typeof InstallTrigger !== 'undefined';

    if(isChrome)
    {
      nick = "Chrome user";
    } 
    if(isFirefox) 
    {
      nick = "isFirefox";
    }

    return nick;
  }

}

export default App;