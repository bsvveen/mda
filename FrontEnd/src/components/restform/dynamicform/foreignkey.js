import React from 'react';
import PropTypes from 'prop-types';

export default class ForeignKey extends React.Component {

    PropTypes = {
        contract : PropTypes.object.isRequired,
        value : PropTypes.string.isRequired,
        repository : PropTypes.object.isRequired,
        constrains : PropTypes.object,
    }

    state = {
        isLoading: true,
        list: []
    }  
    
    constrainValue = () => {
        if (!this.props.constrains || !this.props.contract.relation.constrain || !this.props.constrains[this.props.contract.relation.constrain])
            return undefined;

        return this.props.constrains[this.props.contract.relation.constrain].equals;
    }

    componentDidMount() {          
        this.props.repository.ForeignKey(this.props.contract.key, this.constrainValue()).then(listResponse => {
            this.setState({ list: listResponse }, () => {
                this.setState({ isLoading: false });
            });
        })
    }    

    render() {
        if (this.state.list.length === 0)
            return null;

        var { contract, value, onChange } = this.props;

        var input = this.state.list.map((l) => {
            return (
                <option 
                    className="input"
                    key={l.id}
                    value={l.id}
                    checked={l.id === value}
                >{Object.values(l)[1]}</option>
            );
        });

        return (
            <select value={value} className="input" onChange={(e) => { onChange(e, contract.key) }} {...contract.props}>
                <option value='null'>Selecteer een {this.props.contract.label.replace("_id","")}</option>
                {input}
            </select>
        );
    }
}