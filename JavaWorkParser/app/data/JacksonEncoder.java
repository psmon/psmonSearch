package data;

import org.atmosphere.config.managed.Encoder;
import org.codehaus.jackson.map.ObjectMapper;

import java.io.IOException;

/**
 * Encode a {@link Message} into a String
 */
public class JacksonEncoder implements Encoder<Message, String> {

    private final ObjectMapper mapper = new ObjectMapper();

    @Override
    public String encode(Message m) {
        try {
            return mapper.writeValueAsString(m);
        } catch (IOException e) {
            throw new RuntimeException(e);
        }
    }
}
