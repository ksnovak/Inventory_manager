import React, { Component } from 'react';

export class Inventory extends Component {
    static displayName = Inventory.name;

    constructor(props) {
        super(props);
        this.state = { inventory: [], loading: true, maxOnly: false };

        this.handleMaxToggle = this.handleMaxToggle.bind(this);
    }

    componentDidMount() {
        this.fetchInventoryData();
    }

    static renderInventory(items) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Name</th>
                        <th>Price</th>
                    </tr>
                </thead>
                <tbody>
                    {items.map(item =>
                        <tr key={item.id}>
                            <td>{item.id}</td>
                            <td>{item.name}</td>
                            <td>${item.cost}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    handleMaxToggle(event) {
        this.setState({ maxOnly: event.target.checked }, () => {
            this.fetchInventoryData();
        })
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Inventory.renderInventory(this.state.inventory);

        const { maxOnly }= this.state;

        return (
            <div>
                <h1 id="tabelLabel" >Widget Stocks</h1>
                <p>Are you looking to buy a widget for your family and loved ones?</p>

                <input type="checkbox" id="showMaxOnly" name="showMaxOnly" value={maxOnly} onClick={this.handleMaxToggle} /> <label htmlFor="showMaxOnly">Show most expensive listings only?</label>
                <p>{maxOnly ? 'Here are our most exquisite widget offerings, for the discerning customer' : 'Here are all of the widgets we have for sale, and their greatly affordable prices'}</p>
                {contents}
            </div>
        );
    }

    async fetchInventoryData() {
        const response = await fetch(`api/inventory${this.state.maxOnly ? '/max' : ''} `);
        const data = await response.json();
        this.setState({ inventory: data, loading: false });
    }
}
