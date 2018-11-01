import React, { Component } from 'react';

class BoardVertical extends Component {

    state = {};

    constructor(props) {
        super(props);
        this.state = {...props};
    }

    render() {
        return (
            <div className="vertical">
                <div className="heading">header {this.state.position}</div>
                <div className="cell">0,1</div>
                <div className="cell">0,2</div>
                <div className="cell">0,3</div>
                <div className="cell">0,4</div>
            </div>
        );

    }
}

export default BoardVertical;