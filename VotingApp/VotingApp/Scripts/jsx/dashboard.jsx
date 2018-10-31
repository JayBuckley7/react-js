// the dashboard is the page that shows live results
// all pages pull from the "_Layout.Html
// so this is only the stuff that isnt available on all pages
//jsx files are react code


var dataInit = [
];

var DashboardApp = React.createClass({

	getInitialState: function () {
		return {data: dataInit};
	},
    
    /*
componentDidMount() is invoked immediately after a component is mounted (inserted into the tree). Initialization that requires DOM nodes should go here. If you need to load data from a remote endpoint, this is a good place to instantiate the network request.

This method is a good place to set up any subscriptions. If you do that, don’t forget to unsubscribe in componentWillUnmount().

You may call setState() immediately in componentDidMount().
It will trigger an extra rendering, but it will happen before the browser updates the screen. 
This guarantees that even though the render() will be called twice in this case, the user won’t see the intermediate state. 
Use this pattern with caution because it often causes performance issues. 
In most cases, you should be able to assign the initial state in the constructor() instead. 
It can, however, be necessary for cases like modals and tooltips when you need to measure a DOM node before rendering something that depends on its size or position.
*/
	componentDidMount: function () {
			var self =this;
	    //this is Ajax code, is used to send and retrieve data from servers
	    //asynchronously without interfering with the display and behavior of the existing page.
			  $.ajax({
					 url: this.props.url, //props = properites
					 dataType: 'json',
					 success: function(data2) { 
						  this.setState({data:data2.choices});
					}.bind(this),
					error: function(xhr, status, err) {
						  console.error(this.props.url, status, err.toString()); //default on error code
					}.bind(this)
			 });
			
			//heres that subscription we were talkinging about
			//SignalR Code
			var vhub = $.connection.votingHub;
         
            vhub.client.showLiveResult = function (data) {
				var obj = $.parseJSON(data);	
				self.setState({data: obj});							                
            };	

			$.connection.hub.start();	



	},
    //If you want, the word Tables can be changed here to something else 
	render: function() {
		return (
            <div className="dashboardapp">     		 			  
				<D3PieChart data={this.state.data} title="Tables"/>	   
            </div>
        );
	}

});

//Web api call, returns an Element object representing the element whose id property matches the specified string
React.render(<DashboardApp  url="/home/surveyquiz"  />, document.getElementById('chartResult'));