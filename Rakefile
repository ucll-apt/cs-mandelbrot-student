require 'asciidoctor'


def asciidoc_attributes
  {
    'nofooter' => true,
    'source-highlighter' => 'pygments',
    'stem' => 'latexmath',
    'toc' => 'left',
    'toclevels' => 4,
    'cpp' => 'C++',
  }
end


HTML_FILE = 'assignment.html'
ASCIIDOC_FILE = 'assignment.asciidoc'

desc 'Converts asciidoc to html'
file HTML_FILE => ASCIIDOC_FILE do
  Asciidoctor.convert_file(ASCIIDOC_FILE, safe: :safe, backend: 'html', to_file: HTML_FILE, attributes: asciidoc_attributes)
end


task :default => [ 'assignment.html' ]