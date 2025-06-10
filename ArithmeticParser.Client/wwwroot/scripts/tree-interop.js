window.treeInterop = {
    render: function (elementId, data) {
        const container = document.getElementById("tree-container");
        const width = container.clientWidth;
        const height = container.clientHeight;

        const zoom = d3.zoom()
            .scaleExtent([0.2, 5])
            .on("zoom", function (event) {
                g.attr("transform", event.transform);
            });

        const initialTransform = d3.zoomIdentity
            .translate(width / 2, 50)
            .scale(1);
        
        const svg = d3
            .select("#" + elementId)
            .attr("width", width)
            .attr("height", height)
            .call(zoom);
        
        svg.selectAll("*").remove();

        const root = d3.hierarchy(data, d => d.children);

        const treeLayout = d3.tree().nodeSize([150, 100])
        treeLayout(root);

        const g = svg
            .append("g");

        
        // Links
        g.selectAll(".link")
            .data(root.links())
            .enter()
            .append("line")
            .attr("class", "link")
            .attr("x1", d => d.source.x)
            .attr("y1", d => d.source.y)
            .attr("x2", d => d.target.x)
            .attr("y2", d => d.target.y)
            .attr("stroke", "#ccc")
            .attr("thickness", 3);

        // Nodes
        const node = g.selectAll(".node")
            .data(root.descendants())
            .enter()
            .append("g")
            .attr("class", "node")
            .attr("transform", d => `translate(${d.x},${d.y})`);

        node.append("rect")
            .attr("x", -100 / 2)
            .attr("y", -50/ 2)
            .attr("width", 100)
            .attr("height", 50)
            .attr("rx", 10)
            .attr("ry", 10)
            .attr("fill", "#eaeacd");

        node.append("text")
            .attr("dy", 4)
            .attr("text-anchor", "middle")
            .text(d => d.data.name);
        
        
        svg.call(zoom.transform, initialTransform);
    }
};