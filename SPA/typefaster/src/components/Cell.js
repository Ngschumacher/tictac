import React, { Component } from 'react';

class Cell extends Component {

    

    constructor(props) {
        super(props);
        this.state = {...props};
        
        
        console.log(this.state);
    
        this.state.daysEnum = Object.freeze({"noControl":"white", "player1":"blue", "player2":"gray"});
        this.state.status = this.state.daysEnum.noControl;
    console.log(this.state);
        
    }
    
    render() {
        return (
            <div 
                className={"cell " +  this.state.color} 
                onClick={this.props.onClick}>
                {this.state.mark}
            </div>
        )
    }

    clicking = () => {
        if(this.state.status === this.state.daysEnum.noControl)
        {
            this.state.status = this.state.daysEnum.player1;
        }
        this.setState({color : 'gray'});
        console.log("clicked component");
    }

}

export default Cell;