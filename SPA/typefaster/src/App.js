import React, { Component } from 'react';
import { HubConnectionBuilder } from '@aspnet/signalr';

import Chat from './newComponents/Chat';
import Game from './newComponents/Game';
import Modal from './newComponents/Modal';
import GameStats from './newComponents/gameStats';

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
      challenge : null,
      gameInProgress : false,
    }
}

componentDidMount = () => {
  const hubConnection = new HubConnectionBuilder()
                      .withUrl('http://localhost:5000/chatHub')
                      .build();

    
    // let nick = this.getNick();
    let nick = prompt("Please enter your name", "Harry Potter");

    this.setState({ hubConnection, nick }, () => {
      this.state.hubConnection
        .start()
        .then(() => {
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
      });

      this.state.hubConnection.on('challengeRecieved', (challenge) => {
        this.setState({ 
          challenge : challenge,
          show : true
         });
      });

      this.state.hubConnection.on('updateBoard', (game) => {
        this.setState({
          game : game,
          board : game.board,
          gameStatus : game.gameStatus,
        });
      });

      this.state.hubConnection.on('gameStarting', (game) => {
        this.setState({
          challenge : null,
          game : game,
          board : game.board,
          gameStatus : game.gameStatus,
          gameInProgress : true
        })
      });
    });
}

acceptChallenge(userId, gameId) {
  fetch(`http://localhost:5000/api/game/AcceptChallenge?AccepterId=${userId}&gameId=${gameId}`)
  .then(result => {
  })
}


showStats() {
  var player1Id = this.state.game.player1.id;
  var player2Id = this.state.game.player1.id;

  this.closeGame();

  fetch(`http://localhost:5000/api/game/GetStats?userId=${player1Id}&opponentId=${player2Id}`)
  .then(result => {
    return result.json();
  }).then(json => {
    this.setState({
      gameStats : json
    })
  })

  this.closeGame();
  this.setState({showstats : true });
}
closeGame() {
  this.setState({
    game : {},
    board : {},
    gameStatus : {},
    gameInProgress : false,
  })
}


  render() {
    return (
      <div className="App">
       {this.renderChallengeSection()}
       <div>
        <div className="GameContainer">
          {this.renderGameSection()}
        </div>
        <div className="ChatContainer">
          <Chat 
            user={this.state.user}
            connectedUsers={this.state.connectedUsers}
            challenges={this.state.challenges}
          />
        </div>
       </div>
      </div>
    );
  }

  renderChallengeSection() {
    if(this.state.challenge) { 
      return(
        <Modal 
        show={this.state.challenge} 
        gameId={this.state.challenge.gameId}
        acceptChallenge={this.acceptChallenge.bind(this)} 
        userId={this.state.user.id}
        handleClose={this.hideModal}>
          <h2>{this.state.challenge.challengerName} has challenged you</h2>
        </Modal>
      )
    }
  }

  renderGameSection() {
    if(this.state.gameInProgress) {
      return (  
        <Game 
          user={ this.state.user }
          game={ this.state.game }
          board={ this.state.board }
          gameStatus= { this.state.gameStatus }
        
          showStats={ this.showStats.bind(this) }
          closeGame={ this.closeGame.bind(this) }
        />
      ); 
      } else if (this.state.gameStats) {
        return (
          <GameStats 
            user={ this.state.user }
            gameStats={this.state.gameStats }
          />

        )
      }
    
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