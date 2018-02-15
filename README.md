# annpmml
This is a library for deploying and scoring Artificial Neural Network model <a href="http://dmg.org/pmml/pmml-v4-2-1.html">PMML v4.2</a> implemented in <a href="https://www.ibm.com/products/spss-modeler">IBM SPSS Modeler</a> software.
With this library you can implement a model in software, export the PMML file and deploy the model in your own application.
<h3>How to Use</h3>
<p>In your application code, you construct a <code>NNModel</code> object with path of proper PMML file:</p>
<code>NNModel model = new NNModel("E:/Drug.xml");</code>
<p>Then, you should create a dictionary for your data input:</p>
<code>Dictionary<span><</span>string, object<span>></span> input = new Dictionary<span><</span>string, object<span>></span>();</code>
  <code>input.Add("Age", 23);</code></br>
<code>input.Add("Sex", "F");</code></br>
<code>input.Add("BP", "HIGH");</code></br>
<code>input.Add("Cholesterol", "HIGH");</code></br>
<code>input.Add("Na", 0.792535);</code></br>
<code>input.Add("K", 0.031258);</code></br>
<p> And finally with <code>Predict</code> function you can score your input:</p> 
<code> model.Predict(input) </code></br>
<code>Console.WriteLine(model.Predict(input));</code>
