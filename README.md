# annpmml
This is a <a href="http://dmg.org/pmml/pmml-v4-2-1.html">PMML v4.2</a> library for deploying and scoring Artificial Neural Network model implemented in <a href="https://www.ibm.com/products/spss-modeler">IBM SPSS Modeler</a> software.
With this you can implement a model in software, export the PMML file and deploy the model in your own application.
<h3>How to Use</h3>
<p>In your application code, you construct a <code>NNModel</code> object with path of proper PMML file:</p>
<code>NNModel model = new NNModel("E:/Drug.xml");</code>
<p>After that, you should create a dictionary for your data input:</p>
<code>Dictionary<span><</span>string, object<span>></span> input = new Dictionary<span><</span>string, object<span>></span>();</code>
<code>input.Add("Age", 23);</code>
<code>input.Add("Sex", "F");</code>
<code>input.Add("BP", "HIGH");</code>
<code>input.Add("Cholesterol", "HIGH");</code>
<code>input.Add("Na", 0.792535);</code>
<code>input.Add("K", 0.031258);</code>
  <p> And finally with <code>Predict</code> function you can score your input:</p> 
  <code> model.Predict(input) </code>
  <code>Console.WriteLine(model.Predict(input));</code>
