import React from "react";
import PropTypes from 'prop-types';

export default class ForeignKey extends React.Component {

    PropTypes = {
        model: PropTypes.object.isRequired,
        value: PropTypes.string.isRequired,       
        onChange: PropTypes.func.isRequired,
    }

    state = {
        isLoading: true,
        items: []
    }   
      
    /*constrainValue = () => {
        if (!this.props.constrains || !this.props.contract.relation.constrain || !this.props.constrains[this.props.contract.relation.constrain])
            return undefined;

        return this.props.constrains[this.props.contract.relation.constrain].equals;
    }*/

    componentDidMount() {      
        this._isMounted = true;
        const fk = this.props.model.foreignkey;      
        const repository = new Repository(fk.related);  
        repository.List(["Id", fk.lookup], null).then(response => {
            if (this._isMounted) {
            this.setState({ items: response }, () => {
              this.setState({ isLoading: false });
            })};
          })
    }    

    componentWillUnmount() {
        this._isMounted = false;
      }

    render() {
        if (this.state.items.length === 0)
            return null;

        const { model, value, onChange } = this.props;

        const input = this.state.items.map((l) => {
            return (
                <option 
                    className="input"
                    key={l.Id}
                    value={l.Id}
                    checked={l.Id === value}
                >{Object.values(l)[1]}</option>
            );
        });

        return (
            <select value={value} className="input" onChange={(e) => { onChange(e, model.key) }}>
                <option value='null' key='0'>Selecteer een {model.key}</option>
                {input}
            </select>
        );
    }
}